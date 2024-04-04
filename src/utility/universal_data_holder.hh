#ifndef UNIVERSAL_DATA_HOLDER_HEADER
#define UNIVERSAL_DATA_HOLDER_HEADER

#include <memory>
#include <string>

#include <absl/container/flat_hash_map.h>
#include <rapidjson/document.h>

#include <il2cpp_string.hh>

#include "singleton.hh"

class UniversalData {
public:
    void Initialize(const rapidjson::Document& doc);
    Il2CppString* GetValue(const char* key);
    size_t GetSize();

private:
    absl::flat_hash_map<std::string, Il2CppString*> cached_list_;
};

class UniversalDataBuilder : public Singleton<UniversalDataBuilder> {
public:
    void Register(const char* registered_name, const char* file_name);

    std::shared_ptr<UniversalData> GetByName(const char* name);

private:
    absl::flat_hash_map<std::string, std::shared_ptr<UniversalData>> registered_list_;
};

#endif // universal_data_holder.hh