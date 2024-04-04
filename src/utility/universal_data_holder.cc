#include "universal_data_holder.hh"

#include "utility/configuration.hh"
#include "utility/universal_data_holder.hh"
#include <memory>

#include "logger.hh"

using namespace rapidjson;

void UniversalDataBuilder::Register(const char* registered_name, const char* file_name) {
    if (auto itor = registered_list_.find(registered_name); itor != registered_list_.end()) {
        return;
    }

    auto doc = Utility::LoadJsonFromFile(file_name);

    if (doc.ok()) {
        auto result = std::make_shared<UniversalData>();
        result->Initialize(doc.value());
        registered_list_.emplace(registered_name, result);
        LOGD("[UniversalDataBuilder] Register %s - %lu", registered_name, result->GetSize());
    }
}

static std::shared_ptr<UniversalData> empty_data = std::make_shared<UniversalData>();

std::shared_ptr<UniversalData> UniversalDataBuilder::GetByName(const char* name) {
    if (auto itor = registered_list_.find(name); itor != registered_list_.end()) {
        return itor->second;
    }
    return empty_data;
}

void UniversalData::Initialize(const Document& doc) {
    for (const auto& item : doc.GetObject()) {
        if (item.name.IsNull() == false) {
            if (auto itor = item.value.FindMember("CN"); itor != item.value.MemberEnd() && itor->value.IsNull() == false && itor->value.IsString()) {
                auto key = item.name.GetString();
                auto value = itor->value.GetString();
                auto il_value = Il2CppString::NewString(value);
                cached_list_.emplace(key, il_value);
            }
        }
    }
}

Il2CppString* UniversalData::GetValue(const char* key) {
    if (auto itor = cached_list_.find(key); itor != cached_list_.end()) {
        return itor->second;
    }
    return nullptr;
}

size_t UniversalData::GetSize() {
    return cached_list_.size();
}