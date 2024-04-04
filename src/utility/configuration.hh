#ifndef CONFIG_HEADER
#define CONFIG_HEADER

#include "il2cpp_struct.hh"

#include <absl/container/flat_hash_map.h>
#include <absl/status/statusor.h>
#include <bundle/bundle_consumer.hh>
#include <rapidjson/document.h>

namespace Utility {
    absl::StatusOr<rapidjson::Document> LoadJsonFromFile(const char*);
} // namespace Utility

class Config {
public:
    static void Initialize(std::string config_path);
    static int64_t AppVer;
    static std::string ModConfigPath;

    static Il2CppString* GetBuffName(std::string_view name);
    static Il2CppString* GetBuffDetail(std::string_view name);
    static Il2CppString* GetCommandCodeName(std::string_view name);
    static Il2CppString* GetCraftEssenceName(std::string_view name);
    static Il2CppString* GetCostumeDetail(std::string_view name);
    static Il2CppString* GetCostumeName(std::string_view name);
    static Il2CppString* GetEquipDetail(std::string_view name);
    static Il2CppString* GetEquipName(std::string_view name);
    static Il2CppString* GetEventName(std::string_view detail);
    static Il2CppString* GetItemName(std::string_view name);
    static Il2CppString* GetMissionName(std::string_view name);
    static Il2CppString* GetQuestName(std::string_view detail);
    static Il2CppString* GetShopName(std::string_view name);
    static Il2CppString* GetSkillDetail(std::string_view detail);
    static Il2CppString* GetSkillName(std::string_view name);
    static Il2CppString* GetSpotName(std::string_view detail);
    static Il2CppString* GetSvtName(std::string_view name);
    static Il2CppString* GetTDDetail(std::string_view detail);
    static Il2CppString* GetTDName(std::string_view name);
    static Il2CppString* GetTDRuby(std::string_view name);
    static Il2CppString* GetTDType(std::string_view name);
    static Il2CppString* GetWarName(std::string_view detail);

    static BundleConsumer& GetScriptBundle();
};

#endif // !CONFIG_HEADER