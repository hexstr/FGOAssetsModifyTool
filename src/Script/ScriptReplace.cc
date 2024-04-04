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
#include <il2cpp_string.hh>
#include <il2cpp_struct.hh>


#include "utility/configuration.hh"
#include "utility/logger.hh"

using namespace std;

static BundleConsumer ScriptBundle;

Il2CppString* (*ori_AssetData_GetDecryptObjectText)(intptr_t, Il2CppString*, Il2CppString*) = nullptr;
Il2CppString* new_AssetData_GetDecryptObjectText(intptr_t thisptr, Il2CppString* name, Il2CppString* key) {
    if (name && name->chars[0] > 0x29 && name->chars[0] < 0x40) {
        auto asset_name = Il2CppString::IlStrToStr(name);
        std::string& chinese_text = Config::GetScriptBundle().GetDataById(asset_name.c_str());
        if (chinese_text.length()) {
            auto game_text = Il2CppString::NewString(chinese_text.length());
            std::memcpy(game_text->chars, chinese_text.data(), chinese_text.length());
            return game_text;
        }
    }
    return ori_AssetData_GetDecryptObjectText(thisptr, name, key);
}

void ScriptReplace::Start() {
    info_list_.emplace_back("", "AssetData", "GetDecryptObjectText", "", MakeInfo(AssetData_GetDecryptObjectText));
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