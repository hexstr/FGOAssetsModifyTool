#include "FigureReplace.hh"

#include <cstring>
#include <filesystem>
#include <string_view>
#include <unordered_map>

#include <absl/strings/str_format.h>

#include <bundle/bundle_consumer.hh>
#include <il2cpp_api.hh>
#include <il2cpp_dummy_class.hh>
#include <string_utils.hh>

#include "utility/configuration.hh"
#include "utility/logger.hh"

namespace fs = std::filesystem;
using namespace std;
using namespace Il2CppApi::StringUtils;

struct Figure {
    std::string id_;
    bool enable_;
};

namespace YAML {
    template <>
    struct convert<Figure> {
        static Node encode(const Figure& rhs) {
            Node node;
            node["id"] = rhs.id_;
            node["enable"] = rhs.enable_;
            return node;
        }

        static bool decode(const Node& node, Figure& rhs) {
            if (const auto& itor = node["id"]; itor.IsScalar()) {
                rhs.id_ = itor.as<std::string>();
            }
            if (const auto& itor = node["enable"]; itor.IsScalar()) {
                rhs.enable_ = itor.as<bool>();
            }
            return true;
        }
    };
} // namespace YAML

using LoadImage_f = bool (*)(void*, MyArray<unsigned char>*, bool);
static LoadImage_f ImageConversion_LoadImage = nullptr;

bool LoadImage(void* texture, MyArray<unsigned char>* data, bool mark_non_readable) {
    if (data) {
        return ImageConversion_LoadImage(texture, data, mark_non_readable);
    }
    return false;
}

static unordered_map<std::string, FigureData*> FigureCache;

// 窄边立绘
// NarrowFigure@SvtId
// W x H = 149 x 376
void* (*ori_UINarrowFigureRender_GetBodyTexture)(void*, void*) = nullptr;
void* new_UINarrowFigureRender_GetBodyTexture(void* _, void* assetData) {
    auto& dummy_asset_data = DummyClassBuilder::GetInstance().GetDummyClass(assetData);
    auto asset_name = dummy_asset_data.GetString("name");
    string_view sv(&asset_name[13], asset_name.size() - 13);
    void* res = ori_UINarrowFigureRender_GetBodyTexture(_, assetData);
    if (auto itor = FigureCache.find(sv.data()); itor != FigureCache.end()) {
        auto& dummy_ui_narrow_figure_render = DummyClassBuilder::GetInstance().GetDummyClass(_);
        auto image_limit_count = dummy_ui_narrow_figure_render.Get<int32_t>("imageLimitCount");
        if (image_limit_count == 3) {
            LoadImage(res, itor->second->narrow_.b_, false);
        }
        else {
            LoadImage(res, itor->second->narrow_.a_, false);
        }
    }
    return res;
}

// 从者详细界面
// CharaGraph@SvtId
// W x H = 512 x 725
void* (*ori_UICharaGraphRender_GetBodyTexture)(void*, void*) = nullptr;
void* new_UICharaGraphRender_GetBodyTexture(void* _, void* assetData) {
    static const char bodyTextureNameTable[] = {
        'a',
        'a',
        'b',
        'b',
        'c',
        'c'
    };

    void* res = ori_UICharaGraphRender_GetBodyTexture(_, assetData);

    auto& dummy_ui_chara_graph_render = DummyClassBuilder::GetInstance().GetDummyClass(_);
    auto image_limit_count = dummy_ui_chara_graph_render.Get<int32_t>("imageLimitCount");

    auto suffix = bodyTextureNameTable[image_limit_count];

    auto& dummy_asset_data = DummyClassBuilder::GetInstance().GetDummyClass(assetData);
    auto asset_name = dummy_asset_data.GetString("name");

    string_view sv(&asset_name[11]);

    if (auto itor = FigureCache.find(sv.data()); itor != FigureCache.end()) {
        if (suffix == 'a') {
            LoadImage(res, itor->second->chara_.a_, false);
        }
        else if (suffix == 'b') {
            LoadImage(res, itor->second->chara_.b_, false);
        }
    }
    return res;
}

// Master头像
// MasterFace@EquipId
// W x H = 256 x 256
void* (*ori_UIMasterFaceRender_GetBodyTexture)(intptr_t, intptr_t) = nullptr;
void* new_UIMasterFaceRender_GetBodyTexture(intptr_t thisptr, intptr_t dataList) {
    void* res = ori_UIMasterFaceRender_GetBodyTexture(thisptr, dataList);
    if (auto itor = FigureCache.find("Master");
        itor != FigureCache.end()) {
        LoadImage(res, itor->second->chara_.a_, false);
    }
    return res;
}

// 立绘
// CharaFigure@SvtId
// Texture2D[] GetTextureList(AssetData assetData, bool loadRequiredResource = False) { }
void* (*ori_UIStandFigureRender_GetTextureList)(void*, bool) = nullptr;
void* new_UIStandFigureRender_GetTextureList(void* assetData, bool loadRequiredResource) {
    auto& dummy_asset_data = DummyClassBuilder::GetInstance().GetDummyClass(assetData);
    auto asset_name = dummy_asset_data.GetString("name");
    LOGD("Stand: %s loadRequiredResource: %d", asset_name.c_str(), loadRequiredResource);
    return ori_UIStandFigureRender_GetTextureList(assetData, loadRequiredResource);
}

// Master服装立绘
// UIMasterFigureRenderOld::GetBodyTexture
void* (*ori_UIMasterFigureRenderOld_GetBodyTexture)(intptr_t, intptr_t) = nullptr;
void* new_UIMasterFigureRenderOld_GetBodyTexture(intptr_t thisptr, intptr_t dataList) {
    void* res = ori_UIMasterFigureRenderOld_GetBodyTexture(thisptr, dataList);
    if (auto itor = FigureCache.find("MasterFigure");
        itor != FigureCache.end()) {
        LoadImage(res, itor->second->chara_.a_, false);
    }
    return res;
}
// UIMasterFigureRenderOld::GetBodyAlphaTexture
void* (*ori_UIMasterFigureRenderOld_GetBodyAlphaTexture)(intptr_t, intptr_t) = nullptr;
void* new_UIMasterFigureRenderOld_GetBodyAlphaTexture(intptr_t thisptr, intptr_t dataList) {
    void* res = ori_UIMasterFigureRenderOld_GetBodyAlphaTexture(thisptr, dataList);
    if (auto itor = FigureCache.find("MasterFigureShadow");
        itor != FigureCache.end()) {
        LoadImage(res, itor->second->chara_.a_, false);
    }
    return res;
}

HOOKMETHOD(ServantAssetLoadManager__loadStatusFacelocal, void*, void* _, void* tex, int32_t svtID, int32_t limit) {
    void* res = ori_ServantAssetLoadManager__loadStatusFacelocal(_, tex, svtID, limit);
    std::string svtId = std::to_string(svtID);
    if (auto itor = FigureCache.find(svtId);
        itor != FigureCache.end()) {
        auto& dummy_ui_texture = DummyClassBuilder::GetInstance().GetDummyClass(tex);
        auto m_texture = dummy_ui_texture.Get<void*>("mTexture");

        if (limit == 0) {
            LoadImage(m_texture, itor->second->status_.a_, false);
        }

        else if (limit == 1) {
            LoadImage(m_texture, itor->second->status_.b_, false);
        }

        else if (limit == 3) {
            LoadImage(m_texture, itor->second->status_.c_, false);
        }
    }
    return res;
}

void FigureReplace::Start() {
    info_list_.emplace_back("", "UICharaGraphRender", "GetBodyTexture", "", MakeInfo(UICharaGraphRender_GetBodyTexture));
    info_list_.emplace_back("", "UINarrowFigureRender", "GetBodyTexture", "", MakeInfo(UINarrowFigureRender_GetBodyTexture));
    info_list_.emplace_back("", "UIMasterFaceRender", "GetBodyTexture", "", MakeInfo(UIMasterFaceRender_GetBodyTexture));
    info_list_.emplace_back("", "UIMasterFigureRenderOld", "GetBodyTexture", "", MakeInfo(UIMasterFigureRenderOld_GetBodyTexture));
    info_list_.emplace_back("", "UIMasterFigureRenderOld", "GetBodyAlphaTexture", "", MakeInfo(UIMasterFigureRenderOld_GetBodyAlphaTexture));
    info_list_.emplace_back("", "ServantAssetLoadManager", "loadStatusFacelocal", "", MakeInfo(ServantAssetLoadManager__loadStatusFacelocal));

    ImageConversion_LoadImage = (LoadImage_f)Il2CppApi::ResolveICall("UnityEngine.ImageConversion::LoadImage(UnityEngine.Texture2D,System.Byte[],System.Boolean)");

    // Traverse ModConfigPath to find .chara files
    fs::path mod_path(Config::ModConfigPath);
    for (const fs::directory_entry& dir_entry : fs::recursive_directory_iterator(mod_path)) {
        if (dir_entry.is_directory() == false && dir_entry.path().extension() == ".chara") {
            auto path = dir_entry.path().string();
            LOGD("[FigureReplace] Chara Bundle %s loading...", path.c_str());
            BundleConsumer bundle;
            bundle.Consume(path.c_str());

            for (auto& item : bundle.decoded_data_) {
                const std::string& id = item.first;
                std::string id_num = id.substr(0, id.size() - 1);
                char postfix = id[id.size() - 1];

                auto Chara = MyArray<unsigned char>::NewMyArray(reinterpret_cast<unsigned char*>(item.second.data()), item.second.size());

                if (auto itor = FigureCache.find(id_num); itor != FigureCache.end()) {
                    if (postfix == 'a') {
                        itor->second->chara_.a_ = Chara;
                    }
                    else if (postfix == 'b') {
                        itor->second->chara_.b_ = Chara;
                    }
                }
                else {
                    FigureData* figure_data = nullptr;
                    if (postfix == 'a') {
                        figure_data = new FigureData(nullptr, nullptr, Chara, nullptr);
                    }
                    else if (postfix == 'b') {
                        figure_data = new FigureData(nullptr, nullptr, nullptr, Chara);
                    }
                    if (figure_data) {
                        FigureCache.emplace(id_num, figure_data);
                    }
                }
            }
        }
    }

    try {
        YAML::Node node = YAML::LoadFile(Config::ModConfigPath + "Figure.yaml");
        if (node.IsNull() == false) {
            std::vector<Figure> figure = node.as<std::vector<Figure>>();
            for (auto& item : figure) {
                if (item.enable_) {
                    std::string& id = item.id_;
                    std::string narrow_path_a = absl::StrFormat("%sFigure/NarrowFigure/%s.png", Config::ModConfigPath, id);
                    std::string narrow_path_b = absl::StrFormat("%sFigure/NarrowFigure/%s_2.png", Config::ModConfigPath, id);
                    auto Narrow_a = MyArray<unsigned char>::NewMyArrayFromFile(narrow_path_a.c_str());
                    auto Narrow_b = MyArray<unsigned char>::NewMyArrayFromFile(narrow_path_b.c_str());
                    std::string chara_path_a = absl::StrFormat("%sFigure/CharaGraph/%sa.png", Config::ModConfigPath, id);
                    std::string chara_path_b = absl::StrFormat("%sFigure/CharaGraph/%sb.png", Config::ModConfigPath, id);
                    auto Chara_a = MyArray<unsigned char>::NewMyArrayFromFile(chara_path_a.c_str());
                    auto Chara_b = MyArray<unsigned char>::NewMyArrayFromFile(chara_path_b.c_str());

                    std::string status_path_a = absl::StrFormat("%sFigure/Status/%s_1.png", Config::ModConfigPath, id);
                    std::string status_path_b = absl::StrFormat("%sFigure/Status/%s_2.png", Config::ModConfigPath, id);
                    std::string status_path_c = absl::StrFormat("%sFigure/Status/%s_3.png", Config::ModConfigPath, id);
                    auto Status_a = MyArray<unsigned char>::NewMyArrayFromFile(status_path_a.c_str());
                    auto Status_b = MyArray<unsigned char>::NewMyArrayFromFile(status_path_b.c_str());
                    auto Status_c = MyArray<unsigned char>::NewMyArrayFromFile(status_path_c.c_str());

                    if (Narrow_a || Narrow_b || Chara_a || Chara_b || Status_a || Status_b || Status_c) {
                        auto figure_data = new FigureData(Narrow_a, Narrow_b, Chara_a, Chara_b);
                        figure_data->status_.a_ = Status_a;
                        figure_data->status_.b_ = Status_b;
                        figure_data->status_.c_ = Status_c;
                        FigureCache[id] = figure_data;
                        LOGD("[FigureReplace] %s loaded.", id.c_str());
                    }
                    else {
                        LOGD("[FigureReplace] Failed to load %s.", id.c_str());
                    }
                }
            }
        }
    }

    catch (std::exception& ex) {
        ERROR("[YAML] %s", ex.what());
    }
    {
        auto path = absl::StrFormat("%sFigure/master.png", Config::ModConfigPath);
        auto MasterFace = MyArray<unsigned char>::NewMyArrayFromFile(path.c_str());
        if (MasterFace) {
            FigureCache["Master"] = new FigureData(nullptr, nullptr, MasterFace, nullptr);
        }
    }

    {
        auto path = absl::StrFormat("%sFigure/Gudako_cover.png", Config::ModConfigPath);
        auto MasterFigure = MyArray<unsigned char>::NewMyArrayFromFile(path.c_str());
        if (MasterFigure) {
            FigureCache["MasterFigure"] = new FigureData(nullptr, nullptr, MasterFigure, nullptr);
        }
    }

    {
        auto path = absl::StrFormat("%sFigure/Gudako_shadow.png", Config::ModConfigPath);
        auto MasterFigureShadow = MyArray<unsigned char>::NewMyArrayFromFile(path.c_str());
        if (MasterFigureShadow) {
            FigureCache["MasterFigureShadow"] = new FigureData(nullptr, nullptr, MasterFigureShadow, nullptr);
        }
    }

    LOGI("[FigureReplace] %lu items loaded.", FigureCache.size());
}
