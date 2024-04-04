#include "configuration.hh"

#include <cstdint>
#include <exception>
#include <filesystem>
#include <fstream>
#include <sstream>

namespace fs = std::filesystem;

#include <il2cpp_string.hh>

#include <absl/strings/str_format.h>
#include <rapidjson/error/en.h>
#include <rapidjson/stringbuffer.h>
#include <rapidjson/writer.h>

using namespace rapidjson;

#include "logger.hh"
#include "universal_data_holder.hh"

namespace Utility {
    absl::StatusOr<Document> LoadJsonFromFile(const char* FilePath) {
        Document doc;
        std::ifstream file(FilePath);
        if (file.is_open()) {
            std::string content((std::istreambuf_iterator<char>(file)),
                                (std::istreambuf_iterator<char>()));
            if (doc.Parse(content.c_str()).HasParseError()) {
                if (content.size() > 2) {
                    content[doc.GetErrorOffset() - 2] = '-';
                    content[doc.GetErrorOffset() - 1] = '>';
                }
                ERROR("[%s] %s #%d\nFile: %s %s %zu %s", __FUNCTION__, __FILE__, __LINE__,
                      FilePath, GetParseError_En(doc.GetParseError()), doc.GetErrorOffset(), content.c_str());
                return absl::InternalError("Parse error");
            }
        }
        else {
            ERROR("[%s] %s #%d\nCould not read file: %s.", __FUNCTION__, __FILE__, __LINE__, FilePath);
            return absl::InternalError("Could not read file.");
        }
        return doc;
    }

} // namespace Utility

int64_t Config::AppVer = 0;
std::string Config::ModConfigPath;

BundleConsumer ReadBundle() {
    BundleConsumer ScriptBundle;
    try {
        // Traverse ModConfigPath to find .script files
        fs::path mod_path(Config::ModConfigPath);
        for (const fs::directory_entry& dir_entry :
             fs::recursive_directory_iterator(mod_path)) {
            if (dir_entry.is_directory() == false &&
                dir_entry.path().extension() == ".script") {
                auto path = dir_entry.path().string();
                ScriptBundle.Consume(path.c_str());
            }
        }
        LOGI("[ReadBundle] scripts: %lu", ScriptBundle.GetBundleSize());
    }

    catch (std::exception& ex) {
        ERROR("[ReadBundle] exception: %s", ex.what());
    }
    return ScriptBundle;
}

BundleConsumer& Config::GetScriptBundle() {
    static BundleConsumer consumer = ReadBundle();
    return consumer;
}

void Config::Initialize(std::string config_path) {
    try {
        ModConfigPath = config_path.append("/Mod/");
        if (!fs::exists(ModConfigPath)) {
            fs::create_directories(ModConfigPath);
        }
    }

    catch (std::exception& ex) {
        ERROR("%s#%d %s", __FILE__, __LINE__, __PRETTY_FUNCTION__);
    }
}

auto ReadBuffName() {
    UniversalDataBuilder::GetInstance().Register("buff_names", (Config::ModConfigPath + "buff_names.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("buff_names");
}
Il2CppString* Config::GetBuffName(std::string_view name) {
    static auto data_map = ReadBuffName();
    return data_map->GetValue(name.data());
}

auto ReadBuffDetail() {
    UniversalDataBuilder::GetInstance().Register("buff_detail", (Config::ModConfigPath + "buff_detail.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("buff_detail");
}
Il2CppString* Config::GetBuffDetail(std::string_view name) {
    static auto data_map = ReadBuffDetail();
    return data_map->GetValue(name.data());
}

auto ReadCommandCodeName() {
    UniversalDataBuilder::GetInstance().Register("cc_names", (Config::ModConfigPath + "cc_names.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("cc_names");
}
Il2CppString* Config::GetCommandCodeName(std::string_view name) {
    static auto data_map = ReadCommandCodeName();
    return data_map->GetValue(name.data());
}

auto ReadCraftEssenceName() {
    UniversalDataBuilder::GetInstance().Register("ce_names", (Config::ModConfigPath + "ce_names.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("ce_names");
}
Il2CppString* Config::GetCraftEssenceName(std::string_view name) {
    static auto data_map = ReadCraftEssenceName();
    return data_map->GetValue(name.data());
}

auto ReadCostumeDetail() {
    UniversalDataBuilder::GetInstance().Register("costume_detail", (Config::ModConfigPath + "costume_detail.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("costume_detail");
}
Il2CppString* Config::GetCostumeDetail(std::string_view name) {
    static auto data_map = ReadCostumeDetail();
    return data_map->GetValue(name.data());
}

auto ReadCostumeName() {
    UniversalDataBuilder::GetInstance().Register("costume_names", (Config::ModConfigPath + "costume_names.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("costume_names");
}
Il2CppString* Config::GetCostumeName(std::string_view name) {
    static auto data_map = ReadCostumeName();
    return data_map->GetValue(name.data());
}

auto ReadEquipDetail() {
    UniversalDataBuilder::GetInstance().Register("mc_detail", (Config::ModConfigPath + "mc_detail.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("mc_detail");
}
Il2CppString* Config::GetEquipDetail(std::string_view name) {
    static auto data_map = ReadEquipDetail();
    return data_map->GetValue(name.data());
}

auto ReadEquipName() {
    UniversalDataBuilder::GetInstance().Register("mc_names", (Config::ModConfigPath + "mc_names.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("mc_names");
}
Il2CppString* Config::GetEquipName(std::string_view name) {
    static auto data_map = ReadEquipName();
    return data_map->GetValue(name.data());
}

auto ReadShopName() {
    UniversalDataBuilder::GetInstance().Register("entity_names", (Config::ModConfigPath + "entity_names.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("entity_names");
}
Il2CppString* Config::GetShopName(std::string_view name) {
    static auto data_map = ReadShopName();
    return data_map->GetValue(name.data());
}

auto ReadItemName() {
    UniversalDataBuilder::GetInstance().Register("item_names", (Config::ModConfigPath + "item_names.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("item_names");
}
Il2CppString* Config::GetItemName(std::string_view name) {
    static auto data_map = ReadItemName();
    return data_map->GetValue(name.data());
}

auto ReadMissionName() {
    UniversalDataBuilder::GetInstance().Register("event_mission", (Config::ModConfigPath + "event_mission.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("event_mission");
}
Il2CppString* Config::GetMissionName(std::string_view name) {
    static auto data_map = ReadMissionName();
    return data_map->GetValue(name.data());
}

auto ReadSvtName() {
    UniversalDataBuilder::GetInstance().Register("svt_names", (Config::ModConfigPath + "svt_names.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("svt_names");
}
Il2CppString* Config::GetSvtName(std::string_view name) {
    static auto data_map = ReadSvtName();
    return data_map->GetValue(name.data());
}

auto ReadSkillName() {
    UniversalDataBuilder::GetInstance().Register("skill_names", (Config::ModConfigPath + "skill_names.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("skill_names");
}
Il2CppString* Config::GetSkillName(std::string_view name) {
    static auto data_map = ReadSkillName();
    return data_map->GetValue(name.data());
}

auto ReadSkillDetail() {
    UniversalDataBuilder::GetInstance().Register("skill_detail", (Config::ModConfigPath + "skill_detail.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("skill_detail");
}
Il2CppString* Config::GetSkillDetail(std::string_view name) {
    static auto data_map = ReadSkillDetail();
    return data_map->GetValue(name.data());
}

auto ReadTDName() {
    UniversalDataBuilder::GetInstance().Register("td_names", (Config::ModConfigPath + "td_names.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("td_names");
}
Il2CppString* Config::GetTDName(std::string_view name) {
    static auto data_map = ReadTDName();
    return data_map->GetValue(name.data());
}

auto ReadTDRuby() {
    UniversalDataBuilder::GetInstance().Register("td_ruby", (Config::ModConfigPath + "td_ruby.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("td_ruby");
}
Il2CppString* Config::GetTDRuby(std::string_view name) {
    static auto data_map = ReadTDRuby();
    return data_map->GetValue(name.data());
}

auto ReadTDType() {
    UniversalDataBuilder::GetInstance().Register("td_types", (Config::ModConfigPath + "td_types.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("td_types");
}
Il2CppString* Config::GetTDType(std::string_view name) {
    static auto data_map = ReadTDType();
    return data_map->GetValue(name.data());
}

auto ReadTDDetail() {
    UniversalDataBuilder::GetInstance().Register("td_detail", (Config::ModConfigPath + "td_detail.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("td_detail");
}
Il2CppString* Config::GetTDDetail(std::string_view name) {
    static auto data_map = ReadTDDetail();
    return data_map->GetValue(name.data());
}

auto ReadSpotName() {
    UniversalDataBuilder::GetInstance().Register("spot_names", (Config::ModConfigPath + "spot_names.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("spot_names");
}
Il2CppString* Config::GetSpotName(std::string_view name) {
    static auto data_map = ReadSpotName();
    return data_map->GetValue(name.data());
}

auto ReadQuestName() {
    UniversalDataBuilder::GetInstance().Register("quest_names", (Config::ModConfigPath + "quest_names.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("quest_names");
}
Il2CppString* Config::GetQuestName(std::string_view name) {
    static auto data_map = ReadQuestName();
    return data_map->GetValue(name.data());
}

auto ReadEventName() {
    UniversalDataBuilder::GetInstance().Register("event_names", (Config::ModConfigPath + "event_names.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("event_names");
}
Il2CppString* Config::GetEventName(std::string_view name) {
    static auto data_map = ReadEventName();
    return data_map->GetValue(name.data());
}

auto ReadWarName() {
    UniversalDataBuilder::GetInstance().Register("war_names", (Config::ModConfigPath + "war_names.json").c_str());
    return UniversalDataBuilder::GetInstance().GetByName("War_names");
}
Il2CppString* Config::GetWarName(std::string_view name) {
    static auto data_map = ReadWarName();
    return data_map->GetValue(name.data());
}