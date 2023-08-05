#include "ScriptReplace.hh"

#include <cstring>
#include <filesystem>
#include <fstream>
#include <sstream>
#include <vector>

namespace fs = std::filesystem;

#include <bundle/bundle_consumer.hh>
#include <il2cpp_api.hh>
#include <il2cpp_dummy_class.hh>
#include <il2cpp_struct.hh>
#include <string_utils.hh>

#include "utility/configuration.hh"
#include "utility/logger.hh"

using namespace std;
using namespace Il2CppApi::StringUtils;

static BundleConsumer ScriptBundle;

Il2CppString* (*ori_AssetData_GetDecryptObjectText)(intptr_t, Il2CppString*, Il2CppString*) = nullptr;
Il2CppString* new_AssetData_GetDecryptObjectText(intptr_t thisptr, Il2CppString* name, Il2CppString* key) {
    if (name && name->chars[0] > 0x29 && name->chars[0] < 0x40) {
        auto asset_name = IlStrToStr(name);
        std::string& chinese_text = Config::GetScriptBundle().GetDataById(asset_name.c_str());
        if (chinese_text.length()) {
            auto game_text = NewString(chinese_text.length());
            std::memcpy(game_text->chars, chinese_text.data(), chinese_text.length());
            return game_text;
        }
    }
    return ori_AssetData_GetDecryptObjectText(thisptr, name, key);
}

HOOKMETHOD(ServantVoiceEntity__getFirstGetVoiceList, void*, void* _, int32_t svt_id) {
    auto& dummy_servant_voice_entity = DummyClassBuilder::GetInstance().GetDummyClass(_);
    auto script_json = dummy_servant_voice_entity.Get<MyArray<void*>*>("scriptJson");
    if (script_json) {
        if (script_json->size()) {
            if (auto chinese_infos = Config::GetSvtVoiceData(svt_id)) {
                for (auto script : *script_json) {
                    auto& dummy_svt_voice_info = DummyClassBuilder::GetInstance().GetDummyClass(script);
                    auto infos = dummy_svt_voice_info.Get<MyArray<void*>*>("infos");
                    for (auto info : *infos) {
                        auto& dummy_svt_voice_data = DummyClassBuilder::GetInstance().GetDummyClass(info);
                        auto info_id = dummy_svt_voice_data.GetString("id");
                        if (auto target_infos = std::find_if(chinese_infos->begin(),
                                                             chinese_infos->end(),
                                                             [&](const VoiceInfo& a) { return a.id == info_id; });
                            target_infos != chinese_infos->end()) {
                            dummy_svt_voice_data.Set<Il2CppString*>("text", Il2CppApi::StringUtils::NewString(target_infos->text));
                        }
                    }
                }
            }
        }
    }

    return ori_ServantVoiceEntity__getFirstGetVoiceList(_, svt_id);
}

void ScriptReplace::Start() {
    info_list_.emplace_back("", "AssetData", "GetDecryptObjectText", "", MakeInfo(AssetData_GetDecryptObjectText));
    info_list_.emplace_back("", "ServantVoiceEntity", "getFirstGetVoiceList", "", MakeInfo(ServantVoiceEntity__getFirstGetVoiceList));
    // Traverse ModConfigPath to find .script files
    fs::path mod_path(Config::ModConfigPath);
    for (const fs::directory_entry& dir_entry : fs::recursive_directory_iterator(mod_path)) {
        if (dir_entry.is_directory() == false && dir_entry.path().extension() == ".script") {
            auto path = dir_entry.path().string();
            LOGI("[ScriptReplace] Script bundle %s loading...", path.c_str());
            ScriptBundle.Consume(path.c_str());
        }
    }
    LOGI("[ScriptReplace] Bundle size: %lu", ScriptBundle.GetBundleSize());
}