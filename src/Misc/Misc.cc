#include "Misc.hh"

#include <unistd.h>

#include <filesystem>
#include <fstream>

namespace fs = std::filesystem;

#include <il2cpp_api.hh>
#include <il2cpp_dummy_class.hh>
#include <string_utils.hh>

#include "GameStruct/obfs.hh"
#include "utility/configuration.hh"
#include "utility/logger.hh"

using namespace Il2CppApi::StringUtils;

AssetBundle::LoadFromMemoryAsync_Internal_t AssetBundle::LoadFromMemoryAsync_Internal;
AssetBundle::LoadAssetAsync_Internal_t AssetBundle::LoadAssetAsync_Internal;

AssetBundleRequest::GetAllAssets_t AssetBundleRequest::get_allAssets;

AssetBundleCreateRequest::GetAssetBundle_t AssetBundleCreateRequest::get_assetBundle;

HOOKMETHOD(TitleRootComponent_IsPlayedOpeningMovie, bool, intptr_t) { return true; }

HOOKMETHOD(BestWWWCertVerifyer_IsValid, int, intptr_t, intptr_t, intptr_t) { return 1; }

Il2CppString* (*ori_ServantCommentEntity__GetComment)(void*);
Il2CppString* new_ServantCommentEntity__GetComment(void* _) {
    auto dummy_svt_comment_entity = DummyClassBuilder::GetInstance().GetDummyClass(_);
    auto svtId = dummy_svt_comment_entity.Get<int32_t>("svtId");
    auto id = dummy_svt_comment_entity.Get<int32_t>("id");
    int indexId = svtId * 10 + id;
    auto svt_comment = Config::GetSvtComment();
    if (auto itor = svt_comment.find(indexId); itor != svt_comment.end()) {
        auto new_comment = NewString(itor->second.comment);
        dummy_svt_comment_entity.Set<Il2CppString*>("comment", new_comment);
    }
    return ori_ServantCommentEntity__GetComment(_);
}

static std::atomic<bool> is_loading = false;
static AssetBundle* FGOMainFont = nullptr;
static intptr_t* BackUpFont = nullptr;

void* (*ori_UILabel__get_trueTypeFont)(void**);
void* new_UILabel__get_trueTypeFont(void** _) {
    static auto path = Config::ModConfigPath + "Font";
    static bool is_exist_font = fs::exists(path);

    auto dummy_class_ui_label = DummyClassBuilder::GetInstance().GetDummyClass(_);
    auto mText = dummy_class_ui_label.Get<Il2CppString*>("mText");
    auto mTrueTypeFont = dummy_class_ui_label.Get<void*>("mTrueTypeFont");

    if (is_exist_font && mText && (intptr_t*)mTrueTypeFont != (intptr_t*)FGOMainFont) {
        int size = mText->size();
        if (is_loading == false && FGOMainFont == nullptr && BackUpFont == nullptr && size == 10) {
            is_loading = true;

            auto font_bytes = MyArray<unsigned char>::NewMyArrayFromFile(path.c_str());
            auto font_clazz = Il2CppApi::GetClass("UnityEngine", "Font", Il2CppApi::GetImage("UnityEngine.TextRenderingModule.dll"));
            auto font_type = &font_clazz->byval_arg;
            auto font_object = reinterpret_cast<Il2CppReflectionType*>(Il2CppApi::GetTypeObject(font_type));

            static AssetBundleCreateRequest* bundle_request = AssetBundle::LoadFromMemoryAsync(font_bytes);
            if (bundle_request) {
                if (auto ab = bundle_request->GetAssetBundle()) {
                    auto asset_name = NewString("FGO-Main-Font-Mod");
                    if (auto font = ab->LoadAssetAsync(asset_name, font_object)) {
                        if (auto all_Assets = font->GetAllAssets(); all_Assets->size() == 1) {
                            FGOMainFont = (*all_Assets)[0];
                            BackUpFont = (intptr_t*)mTrueTypeFont;
                        }
                    }
                }
            }
        }

        if (BackUpFont == (intptr_t*)mTrueTypeFont) {
            auto class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "UILabel");
            using set_trueTypeFont_f = void (*)(void*, AssetBundle*);
            static set_trueTypeFont_f set_trueTypeFont = (set_trueTypeFont_f)class_wrapper->GetMethod("set_trueTypeFont");
            if (set_trueTypeFont) {
                set_trueTypeFont(_, FGOMainFont);
            }
        }
    }
    return ori_UILabel__get_trueTypeFont(_);
}

HOOKMETHOD(ServantEntity__getName, Il2CppString*, void* _, int32_t limitCount, int imageLimitCount) {
    auto dummy_svt_entity = DummyClassBuilder::GetInstance().GetDummyClass(_);
    auto id = dummy_svt_entity.Get<ObscuredInt>("id");
    auto svt = Config::GetSvtName();
    if (auto itor = svt.find(id); itor != svt.end()) {
        return NewString(itor->second.name);
    }
    return ori_ServantEntity__getName(_, limitCount, imageLimitCount);
}

HOOKMETHOD(ServantEntity__getBattleName, Il2CppString*, void* _, bool isTrueNameForce, int limitCount) {
    auto dummy_svt_entity = DummyClassBuilder::GetInstance().GetDummyClass(_);
    auto id = dummy_svt_entity.Get<ObscuredInt>("id");
    auto svt = Config::GetSvtName();
    if (auto itor = svt.find(id); itor != svt.end()) {
        return NewString(itor->second.battleName);
    }
    return ori_ServantEntity__getBattleName(_, isTrueNameForce, limitCount);
}

HOOKMETHOD(ServantEntity__getClassName, Il2CppString*, void* _) {
    auto dummy_svt_entity = DummyClassBuilder::GetInstance().GetDummyClass(_);
    auto classId = dummy_svt_entity.Get<int32_t>("classId");
    switch (classId) {
    case 1:
        return NewString("Saber");
    case 2:
        return NewString("Archer");
    case 3:
        return NewString("Lancer");
    case 4:
        return NewString("Rider");
    case 5:
        return NewString("Caster");
    case 6:
        return NewString("Assassin");
    case 7:
    case 0x6B:
        return NewString("Berserker");
    case 8:
        return NewString("Shielder");
    case 9:
        return NewString("Ruler");
    case 0xA:
        return NewString("Alterego");
    case 0xB:
        return NewString("Avenger");
    case 0x11:
        return NewString("Grand Caster");
    case 0x14:
        return NewString("Beast Ⅱ");
    case 0x16:
        return NewString("Beast Ⅰ");
    case 0x17:
        return NewString("Moon Cancer");
    case 0x18:
        return NewString("Beast Ⅲ / R");
    case 0x19:
        return NewString("Foreigner");
    case 0x1A:
        return NewString("Beast Ⅲ / L");
    case 0x1C:
        return NewString("Pretender");
    case 0x1D:
        return NewString("Beast Ⅳ");
    case 0x3E8:
        return NewString("OTHER");
    case 0x3E9:
        return NewString("ALL");
    case 0x3EA:
        return NewString("EXTRA");
    default:
        return NewString("?");
    }
}

HOOKMETHOD(LocalizationManager__SetTextData, void, void* _, Il2CppString* text) {
    static bool is_loaded = false;
    if (is_loaded == false) {
        is_loaded = true;
        LOGI("[LocalizationManager] Loading chinese text.");
        auto path = Config::ModConfigPath + "LocalizationJpn.txt";
        if (Il2CppString* file_content = OpenUTF16File(path.c_str())) {
            if (_ == nullptr) {
                auto c_LocalizationManager = Il2CppApi::GetClass("", "LocalizationManager");
                auto dummy_singleton_mono_behaviour = DummyClassBuilder::GetInstance().GetDummyClass(c_LocalizationManager);
                auto instance = dummy_singleton_mono_behaviour.GetStatic<void*>("instance");
                ori_LocalizationManager__SetTextData(instance, file_content);
            }
            else {
                ori_LocalizationManager__SetTextData(_, file_content);
            }
        }
        else {
            ERROR("Failed to open file LocalizationJpn.txt.");
            if (_) {
                ori_LocalizationManager__SetTextData(_, text);
            }
        }
    }
}

HOOKMETHOD(SkillDetailEntity__getDetail, Il2CppString*, void* _) {
    auto dummy_skill_detail_entity = DummyClassBuilder::GetInstance().GetDummyClass(_);
    auto id = dummy_skill_detail_entity.Get<int32_t>("id");
    if (auto detail = Config::GetSkillDetail(id)) {
        dummy_skill_detail_entity.Set<Il2CppString*>("detail", detail);
    }
    return ori_SkillDetailEntity__getDetail(_);
}

HOOKMETHOD(SkillEntity__getName, Il2CppString*, void* _) {
    auto dummy_mst_name_entity = DummyClassBuilder::GetInstance().GetDummyClass(_);
    auto id = dummy_mst_name_entity.Get<int32_t>("id");
    if (auto name = Config::GetSkillName(id)) {
        return name;
    }
    return ori_SkillEntity__getName(_);
}

HOOKMETHOD(TreasureDvcEntity__getEffectExplanation, bool,
           void* _,
           Il2CppString** tdName,
           Il2CppString** tdExplanation,
           int32_t* maxLv,
           int32_t* tdGuageCount,
           int32_t* tdCardId,
           int32_t* tdStrengthStatus,
           int32_t tdLv) {
    auto dummy_mst_name_entity = DummyClassBuilder::GetInstance().GetDummyClass(_);
    auto id = dummy_mst_name_entity.Get<int32_t>("id");
    auto svt = Config::GetTDName();
    if (auto itor = svt.find(id); itor != svt.end()) {
        dummy_mst_name_entity.Set<Il2CppString*>("name", NewString(itor->second.name));
        dummy_mst_name_entity.Set<Il2CppString*>("ruby", NewString(itor->second.ruby));
        dummy_mst_name_entity.Set<Il2CppString*>("typeText", NewString(itor->second.typeText));
    }
    return ori_TreasureDvcEntity__getEffectExplanation(_, tdName, tdExplanation, maxLv, tdGuageCount, tdCardId, tdStrengthStatus, tdLv);
}

HOOKMETHOD(TreasureDvcDetailEntity__getDetail, Il2CppString*, void* _) {
    auto dummy_mst_detail_entity = DummyClassBuilder::GetInstance().GetDummyClass(_);
    auto id = dummy_mst_detail_entity.Get<int32_t>("id");
    auto svt = Config::GetTDDetail();
    if (auto itor = svt.find(id); itor != svt.end()) {
        return NewString(itor->second.detail);
    }
    return ori_TreasureDvcDetailEntity__getDetail(_);
}

HOOKMETHOD(TreasureDvcDetailEntity__getDetailShort, Il2CppString*, void* _) {
    auto svt = Config::GetTDDetail();
    auto dummy_mst_detail_entity = DummyClassBuilder::GetInstance().GetDummyClass(_);
    auto id = dummy_mst_detail_entity.Get<int32_t>("id");
    if (auto itor = svt.find(id); itor != svt.end()) {
        return NewString(itor->second.detail);
    }
    return ori_TreasureDvcDetailEntity__getDetailShort(_);
}

HOOKMETHOD(TweenAlpha__Begin, void*, void* go, float duration, float alpha) {
    duration *= 0.3;
    return ori_TweenAlpha__Begin(go, duration, alpha);
}

void Misc::LoadInternationalization() {
    new_LocalizationManager__SetTextData(nullptr, nullptr);
}

void Misc::Start() {
    AssetBundle::LoadFromMemoryAsync_Internal = (AssetBundle::LoadFromMemoryAsync_Internal_t)Il2CppApi::ResolveICall("UnityEngine.AssetBundle::LoadFromMemoryAsync_Internal(System.Byte[],System.UInt32)");
    AssetBundleCreateRequest::get_assetBundle = (AssetBundleCreateRequest::GetAssetBundle_t)Il2CppApi::ResolveICall("UnityEngine.AssetBundleCreateRequest::get_assetBundle()");
    AssetBundleRequest::get_allAssets = (AssetBundleRequest::GetAllAssets_t)Il2CppApi::ResolveICall("UnityEngine.AssetBundleRequest::get_allAssets()");
    AssetBundle::LoadAssetAsync_Internal = (AssetBundle::LoadAssetAsync_Internal_t)Il2CppApi::ResolveICall("UnityEngine.AssetBundle::LoadAssetAsync_Internal(System.String,System.Type)");

    info_list_.emplace_back("", "TitleRootComponent", "IsPlayedOpeningMovie", "", MakeInfo(TitleRootComponent_IsPlayedOpeningMovie));

    info_list_.emplace_back("", "ServantCommentEntity", "GetComment", "", MakeInfo(ServantCommentEntity__GetComment));

    info_list_.emplace_back("", "ServantEntity", "getName", "", MakeInfo(ServantEntity__getName));

    info_list_.emplace_back("", "ServantEntity", "getBattleName", "", MakeInfo(ServantEntity__getBattleName), 2);

    info_list_.emplace_back("", "ServantEntity", "getClassName", "", MakeInfo(ServantEntity__getClassName));

    info_list_.emplace_back("", "UILabel", "get_trueTypeFont", "", MakeInfo(UILabel__get_trueTypeFont));

    std::ifstream file("/proc/self/cmdline");
    std::string cmdline;
    std::getline(file, cmdline, '\0');
    file.close();

    if (cmdline == "com.aniplex.fategrandorder") {
        info_list_.emplace_back("", "LocalizationManager", "SetTextData", "", MakeInfo(LocalizationManager__SetTextData));
        info_list_.emplace_back("", "TweenAlpha", "Begin", "", MakeInfo(TweenAlpha__Begin));

        if (fs::exists(Config::ModConfigPath + "sniffer")) {
            info_list_.emplace_back("DelightWorks.Network", "BestWWWCertVerifyer", "IsValid", "", MakeInfo(BestWWWCertVerifyer_IsValid));
        }
    }

    info_list_.emplace_back("", "SkillDetailEntity", "getDetail", "", MakeInfo(SkillDetailEntity__getDetail));

    info_list_.emplace_back("", "SkillEntity", "getName", "", MakeInfo(SkillEntity__getName));

    info_list_.emplace_back("", "TreasureDvcEntity", "getEffectExplanation", "", MakeInfo(TreasureDvcEntity__getEffectExplanation));

    info_list_.emplace_back("", "TreasureDvcDetailEntity", "getDetail", "", MakeInfo(TreasureDvcDetailEntity__getDetail));

    info_list_.emplace_back("", "TreasureDvcDetailEntity", "getDetailShort", "", MakeInfo(TreasureDvcDetailEntity__getDetailShort));
}

void Misc::Hook() {
    HookTemplate::Hook();
    LoadInternationalization();
}