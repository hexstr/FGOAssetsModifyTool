#include "Misc.hh"

#include <unistd.h>

#include <filesystem>
#include <fstream>

namespace fs = std::filesystem;

#include <il2cpp_api.hh>
#include <il2cpp_dummy_class.hh>
#include <il2cpp_string.hh>

#include "GameStruct/obfs.hh"
#include "dictionary.hh"
#include "utility/configuration.hh"
#include "utility/logger.hh"

AssetBundle::LoadFromMemoryAsync_Internal_t AssetBundle::LoadFromMemoryAsync_Internal;
AssetBundle::LoadAssetAsync_Internal_t AssetBundle::LoadAssetAsync_Internal;

AssetBundleRequest::GetAllAssets_t AssetBundleRequest::get_allAssets;

AssetBundleCreateRequest::GetAssetBundle_t AssetBundleCreateRequest::get_assetBundle;

HOOKMETHOD(TitleRootComponent_IsPlayedOpeningMovie, bool, intptr_t) { return true; }

HOOKMETHOD(BestWWWCertVerifyer_IsValid, int, intptr_t, intptr_t, intptr_t) { return 1; }

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
                    auto asset_name = Il2CppString::NewString("FGO-Main-Font-Mod");
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

HOOKMETHOD(ServantEntity__getClassName, Il2CppString*, void* _) {
    auto dummy_svt_entity = DummyClassBuilder::GetInstance().GetDummyClass(_);
    auto classId = dummy_svt_entity.Get<int32_t>("classId");
    switch (classId) {
        case 1:
            return Il2CppString::NewString("Saber");
        case 2:
            return Il2CppString::NewString("Archer");
        case 3:
            return Il2CppString::NewString("Lancer");
        case 4:
            return Il2CppString::NewString("Rider");
        case 5:
            return Il2CppString::NewString("Caster");
        case 6:
            return Il2CppString::NewString("Assassin");
        case 7:
        case 0x6B:
            return Il2CppString::NewString("Berserker");
        case 8:
            return Il2CppString::NewString("Shielder");
        case 9:
            return Il2CppString::NewString("Ruler");
        case 0xA:
            return Il2CppString::NewString("Alterego");
        case 0xB:
            return Il2CppString::NewString("Avenger");
        case 0x11:
            return Il2CppString::NewString("Grand Caster");
        case 0x14:
            return Il2CppString::NewString("Beast Ⅱ");
        case 0x16:
            return Il2CppString::NewString("Beast Ⅰ");
        case 0x17:
            return Il2CppString::NewString("Moon Cancer");
        case 0x18:
            return Il2CppString::NewString("Beast Ⅲ / R");
        case 0x19:
            return Il2CppString::NewString("Foreigner");
        case 0x1A:
            return Il2CppString::NewString("Beast Ⅲ / L");
        case 0x1C:
            return Il2CppString::NewString("Pretender");
        case 0x1D:
            return Il2CppString::NewString("Beast Ⅳ");
        case 0x3E8:
            return Il2CppString::NewString("OTHER");
        case 0x3E9:
            return Il2CppString::NewString("ALL");
        case 0x3EA:
            return Il2CppString::NewString("EXTRA");
        default:
            return Il2CppString::NewString("?");
    }
}

static bool is_loaded = false;
HOOKMETHOD(LocalizationManager__SetTextData, void, void* _, Il2CppString* text) {
    if (is_loaded == false) {
        is_loaded = true;
        LOGI("[FGOAssetReplace] Loading...\nCompiled at: " __DATE__ " | " __TIME__);
        auto path = Config::ModConfigPath + "LocalizationJpn.txt";
        if (Il2CppString* file_content = Il2CppString::OpenUTF16File(path.c_str())) {
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

HOOKMETHOD(TweenAlpha__Begin, void*, void* go, float duration, float alpha) {
    duration *= 0.3;
    return ori_TweenAlpha__Begin(go, duration, alpha);
}

void Misc::LoadInternationalization() {
    new_LocalizationManager__SetTextData(nullptr, nullptr);
}

HOOKMETHOD(CommonUI__InitMaskClick, void, void* _) {
    auto target_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "DataManager");
    auto target_method = target_class_wrapper->GetMethodInfo("GetMasterData");
    auto& dummy_singleton_mono_behaviour = DummyClassBuilder::GetInstance().GetDummyClass(target_class_wrapper->class_ptr_);
    auto instance = dummy_singleton_mono_behaviour.GetStatic<void*>("instance");

    Il2CppException* exception;
    void* args = nullptr;

    {
        auto costume_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "ServantCostumeMaster");
        auto costume_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ costume_class_wrapper->class_ptr_ });
        auto costume_master_data = Il2CppApi::RuntimeInvoke(costume_method, instance, &args, &exception);
        auto& dummy_costume = DummyClassBuilder::GetInstance().GetDummyClass(costume_master_data);
        auto list = dummy_costume.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto costume_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (costume_list->_size > 0) {
            LOGD("costume_list: %d", costume_list->_size);
            auto item = costume_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);

                    auto name = dummy_element.GetString("name");
                    if (auto cn_name = Config::GetCostumeName(name)) {
                        dummy_element.Set("name", cn_name);
                    }
                }

                // auto ruby = dummy_element.GetString("detail");
                // if (auto cn_ruby = Config::GetCostumeDetail(ruby)) {
                //     dummy_element.Set("detail", cn_ruby);
                // }
            }
        }
    }

    {
        auto equip_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "EquipMaster");
        auto equip_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ equip_class_wrapper->class_ptr_ });
        auto equip_master_data = Il2CppApi::RuntimeInvoke(equip_method, instance, &args, &exception);
        auto& dummy_equip = DummyClassBuilder::GetInstance().GetDummyClass(equip_master_data);
        auto list = dummy_equip.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto equip_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (equip_list->_size > 0) {
            LOGD("equip_list: %d", equip_list->_size);
            auto item = equip_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto name = dummy_element.Get<ObscuredString*>("name");
                    // auto detail = dummy_element.Get<ObscuredString*>("detail");

                    if (name->hiddenValue && name->hiddenValue->size()) {
                        auto name_str = name->str();
                        if (auto cn_name = Config::GetEquipName(name_str)) {
                            auto replaced_hidden_value = MyArray<unsigned char>::NewMyArray(cn_name->size() * 2);

                            cn_name->obfs(replaced_hidden_value);
                            name->CloneClass(replaced_hidden_value);

                            name->hiddenValue = replaced_hidden_value;
                        }
                    }
                }

                // if (detail->hiddenValue && detail->hiddenValue->size()) {
                //     auto detail_str = detail->str();
                //     if (auto cn_detail = Config::GetEquipDetail(detail_str)) {
                //         auto replaced_hidden_value = MyArray<unsigned char>::NewMyArray(cn_detail->size() * 2);

                //         cn_detail->obfs(replaced_hidden_value);
                //         detail->CloneClass(replaced_hidden_value);

                //         detail->hiddenValue = replaced_hidden_value;
                //     }
                // }
            }
        }
    }

    {
        auto td_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "TreasureDvcMaster");
        auto td_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ td_class_wrapper->class_ptr_ });
        auto td_master_data = Il2CppApi::RuntimeInvoke(td_method, instance, &args, &exception);
        auto& dummy_td = DummyClassBuilder::GetInstance().GetDummyClass(td_master_data);
        auto list = dummy_td.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto td_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (td_list->_size > 0) {
            LOGD("td_list: %d", td_list->_size);
            auto item = td_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);

                    auto name = dummy_element.GetString("name");
                    if (auto cn_name = Config::GetTDName(name)) {
                        dummy_element.Set("name", cn_name);
                    }

                    auto ruby = dummy_element.GetString("ruby");
                    if (auto cn_ruby = Config::GetTDRuby(ruby)) {
                        dummy_element.Set("ruby", cn_ruby);
                    }

                    auto type = dummy_element.GetString("typeText");
                    if (auto cn_type = Config::GetTDType(type)) {
                        dummy_element.Set("typeText", cn_type);
                    }
                }
            }
        }
    }

    {
        auto td_detail_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "TreasureDvcDetailMaster");
        auto td_detail_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ td_detail_class_wrapper->class_ptr_ });
        auto td_detail_master_data = Il2CppApi::RuntimeInvoke(td_detail_method, instance, &args, &exception);
        auto& dummy_td_detail = DummyClassBuilder::GetInstance().GetDummyClass(td_detail_master_data);
        auto list = dummy_td_detail.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto td_detail_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (td_detail_list->_size > 0) {
            LOGD("td_detail_list: %d", td_detail_list->_size);
            auto item = td_detail_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto detail = dummy_element.GetString("detail");

                    if (auto cn_detail = Config::GetTDDetail(detail)) {
                        dummy_element.Set("detail", cn_detail);
                    }

                    auto detail_short = dummy_element.GetString("detailShort");

                    if (auto cn_detail_short = Config::GetTDDetail(detail_short)) {
                        dummy_element.Set("detailShort", cn_detail_short);
                    }
                }
            }
        }
    }

    {
        auto skill_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "SkillMaster");
        auto skill_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ skill_class_wrapper->class_ptr_ });
        auto skill_master_data = Il2CppApi::RuntimeInvoke(skill_method, instance, &args, &exception);
        auto& dummy_skill = DummyClassBuilder::GetInstance().GetDummyClass(skill_master_data);
        auto list = dummy_skill.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto skill_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (skill_list->_size > 0) {
            LOGD("skill_list: %d", skill_list->_size);
            auto item = skill_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto name = dummy_element.GetString("name");

                    if (auto cn_name = Config::GetSkillName(name)) {
                        dummy_element.Set("name", cn_name);
                    }
                    else if (auto cn_name = Config::GetCommandCodeName(name)) {
                        dummy_element.Set("name", cn_name);
                    }
                    else if (auto cn_name = Config::GetCraftEssenceName(name)) {
                        dummy_element.Set("name", cn_name);
                    }
                }
            }
        }
    }

    {
        auto skill_detail_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "SkillDetailMaster");
        auto skill_detail_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ skill_detail_class_wrapper->class_ptr_ });
        auto skill_detail_master_data = Il2CppApi::RuntimeInvoke(skill_detail_method, instance, &args, &exception);
        auto& dummy_skill_detail = DummyClassBuilder::GetInstance().GetDummyClass(skill_detail_master_data);
        auto list = dummy_skill_detail.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto skill_detail_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (skill_detail_list->_size > 0) {
            LOGD("skill_detail_list: %d", skill_detail_list->_size);
            auto item = skill_detail_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto detail = dummy_element.GetString("detail");

                    if (auto cn_detail = Config::GetSkillDetail(detail)) {
                        dummy_element.Set("detail", cn_detail);
                    }

                    auto detail_short = dummy_element.GetString("detailShort");

                    if (auto cn_detail_short = Config::GetSkillDetail(detail_short)) {
                        dummy_element.Set("detailShort", cn_detail_short);
                    }
                }
            }
        }
    }

    {
        auto spot_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "SpotMaster");
        auto spot_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ spot_class_wrapper->class_ptr_ });
        auto spot_master_data = Il2CppApi::RuntimeInvoke(spot_method, instance, &args, &exception);
        auto& dummy_spot = DummyClassBuilder::GetInstance().GetDummyClass(spot_master_data);
        auto list = dummy_spot.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto spot_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (spot_list->_size > 0) {
            LOGD("spot_list: %d", spot_list->_size);
            auto item = spot_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto name = dummy_element.GetString("name");

                    if (auto cn_name = Config::GetSpotName(name)) {
                        dummy_element.Set("name", cn_name);
                    }
                }
            }
        }
    }

    {
        auto quest_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "QuestMaster");
        auto quest_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ quest_class_wrapper->class_ptr_ });
        auto quest_master_data = Il2CppApi::RuntimeInvoke(quest_method, instance, &args, &exception);
        auto& dummy_quest = DummyClassBuilder::GetInstance().GetDummyClass(quest_master_data);
        auto list = dummy_quest.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto quest_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (quest_list->_size > 0) {
            LOGD("quest_list: %d", quest_list->_size);
            auto item = quest_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto name = dummy_element.GetString("name");

                    if (auto cn_name = Config::GetQuestName(name)) {
                        dummy_element.Set("name", cn_name);
                    }
                }
            }
        }
    }

    {
        auto shop_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "ShopMaster");
        auto shop_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ shop_class_wrapper->class_ptr_ });
        auto shop_master_data = Il2CppApi::RuntimeInvoke(shop_method, instance, &args, &exception);
        auto& dummy_shop = DummyClassBuilder::GetInstance().GetDummyClass(shop_master_data);
        auto list = dummy_shop.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto shop_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (shop_list->_size > 0) {
            LOGD("shop_list: %d", shop_list->_size);
            auto item = shop_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto name = dummy_element.GetString("name");

                    if (auto cn_name = Config::GetShopName(name)) {
                        dummy_element.Set("name", cn_name);
                    }
                    else if (auto cn_name = Config::GetItemName(name)) {
                        dummy_element.Set("name", cn_name);
                    }
                    else if (auto cn_name = Config::GetCraftEssenceName(name)) {
                        dummy_element.Set("name", cn_name);
                    }
                    else if (auto cn_name = Config::GetCommandCodeName(name)) {
                        dummy_element.Set("name", cn_name);
                    }
                }
            }
        }
    }

    {
        auto item_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "ItemMaster");
        auto item_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ item_class_wrapper->class_ptr_ });
        auto item_master_data = Il2CppApi::RuntimeInvoke(item_method, instance, &args, &exception);
        auto& dummy_item = DummyClassBuilder::GetInstance().GetDummyClass(item_master_data);
        auto list = dummy_item.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto item_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (item_list->_size > 0) {
            LOGD("item_list: %d", item_list->_size);
            auto item = item_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto name = dummy_element.GetString("name");

                    if (auto cn_name = Config::GetItemName(name)) {
                        dummy_element.Set("name", cn_name);
                    }
                }
            }
        }
    }

    {
        auto event_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "EventMaster");
        auto event_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ event_class_wrapper->class_ptr_ });
        auto event_master_data = Il2CppApi::RuntimeInvoke(event_method, instance, &args, &exception);
        auto& dummy_event = DummyClassBuilder::GetInstance().GetDummyClass(event_master_data);
        auto list = dummy_event.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto event_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (event_list->_size > 0) {
            LOGD("event_list: %d", event_list->_size);
            auto item = event_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto name = dummy_element.GetString("name");

                    if (auto cn_name = Config::GetEventName(name)) {
                        dummy_element.Set("name", cn_name);
                    }
                }
            }
        }
    }

    {
        auto war_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "WarMaster");
        auto war_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ war_class_wrapper->class_ptr_ });
        auto war_master_data = Il2CppApi::RuntimeInvoke(war_method, instance, &args, &exception);
        auto& dummy_war = DummyClassBuilder::GetInstance().GetDummyClass(war_master_data);
        auto list = dummy_war.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto war_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (war_list->_size > 0) {
            LOGD("war_list: %d", war_list->_size);
            auto item = war_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto name = dummy_element.GetString("name");

                    if (auto cn_name = Config::GetWarName(name)) {
                        dummy_element.Set("name", cn_name);
                    }
                }
            }
        }
    }

    {
        auto servant_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "ServantMaster");
        auto servant_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ servant_class_wrapper->class_ptr_ });
        auto servant_master_data = Il2CppApi::RuntimeInvoke(servant_method, instance, &args, &exception);
        auto& dummy_servant = DummyClassBuilder::GetInstance().GetDummyClass(servant_master_data);
        auto list = dummy_servant.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto servant_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (servant_list->_size > 0) {
            LOGD("servant_list: %d", servant_list->_size);
            auto item = servant_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto battle_name = dummy_element.GetString("battleName");
                    auto name_save = dummy_element.Get<ObscuredString*>("nameSave");

                    if (name_save->hiddenValue && name_save->hiddenValue->size()) {
                        auto name = name_save->str();
                        if (auto cn_name = Config::GetSvtName(name)) {
                            auto replaced_hidden_value = MyArray<unsigned char>::NewMyArray(cn_name->size() * 2);

                            cn_name->obfs(replaced_hidden_value);
                            name_save->CloneClass(replaced_hidden_value);

                            name_save->hiddenValue = replaced_hidden_value;
                        }
                        else if (auto cn_name = Config::GetCraftEssenceName(name)) {
                            auto replaced_hidden_value = MyArray<unsigned char>::NewMyArray(cn_name->size() * 2);

                            cn_name->obfs(replaced_hidden_value);
                            name_save->CloneClass(replaced_hidden_value);

                            name_save->hiddenValue = replaced_hidden_value;
                        }
                        else if (auto cn_name = Config::GetShopName(name)) {
                            auto replaced_hidden_value = MyArray<unsigned char>::NewMyArray(cn_name->size() * 2);

                            cn_name->obfs(replaced_hidden_value);
                            name_save->CloneClass(replaced_hidden_value);

                            name_save->hiddenValue = replaced_hidden_value;
                        }
                    }

                    if (auto cn_name = Config::GetSvtName(battle_name)) {
                        dummy_element.Set("battleName", cn_name);
                    }
                }
            }
        }
    }

    {
        auto command_code_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "CommandCodeMaster");
        auto command_code_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ command_code_class_wrapper->class_ptr_ });
        auto command_code_master_data = Il2CppApi::RuntimeInvoke(command_code_method, instance, &args, &exception);
        auto& dummy_command_code = DummyClassBuilder::GetInstance().GetDummyClass(command_code_master_data);
        auto list = dummy_command_code.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto command_code_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (command_code_list->_size > 0) {
            LOGD("command_code_list: %d", command_code_list->_size);
            auto item = command_code_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto name = dummy_element.Get<ObscuredString*>("name");
                    auto ruby = dummy_element.Get<ObscuredString*>("ruby");

                    if (name->hiddenValue && name->hiddenValue->size()) {
                        auto name_str = name->str();
                        if (auto cn_name = Config::GetCommandCodeName(name_str)) {
                            auto replaced_hidden_value = MyArray<unsigned char>::NewMyArray(cn_name->size() * 2);

                            cn_name->obfs(replaced_hidden_value);
                            name->CloneClass(replaced_hidden_value);

                            name->hiddenValue = replaced_hidden_value;
                            ruby->hiddenValue = replaced_hidden_value;
                        }
                    }
                }
            }
        }
    }

    {
        auto servant_limit_add_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "ServantLimitAddMaster");
        auto servant_limit_add_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ servant_limit_add_class_wrapper->class_ptr_ });
        auto servant_limit_add_master_data = Il2CppApi::RuntimeInvoke(servant_limit_add_method, instance, &args, &exception);
        auto& dummy_servant_limit_add = DummyClassBuilder::GetInstance().GetDummyClass(servant_limit_add_master_data);
        auto list = dummy_servant_limit_add.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto servant_limit_add_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (servant_limit_add_list->_size > 0) {
            LOGD("servant_limit_add_list: %d", servant_limit_add_list->_size);
            const auto& item = servant_limit_add_list->_items;
            for (auto& element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto servant_limit_add_dict = dummy_element.Get<Dictionary<Il2CppString*, Il2CppObject*>*>("script");
                    auto entries_dict = servant_limit_add_dict->entries;
                    if (entries_dict && entries_dict->size()) {
                        for (auto& item : *entries_dict) {
                            if (item.value) {
                                if (std::string_view(item.value->klass->name) == "String") {
                                    auto item_str = reinterpret_cast<Il2CppString*>(item.value)->str();
                                    if (auto cn_name = Config::GetSvtName(item_str)) {
                                        item.value = reinterpret_cast<Il2CppObject*>(cn_name);
                                    }
                                    else if (auto cn_name = Config::GetSkillName(item_str)) {
                                        item.value = reinterpret_cast<Il2CppObject*>(cn_name);
                                    }
                                    else if (auto cn_name = Config::GetTDName(item_str)) {
                                        item.value = reinterpret_cast<Il2CppObject*>(cn_name);
                                    }
                                    else if (auto cn_name = Config::GetTDRuby(item_str)) {
                                        item.value = reinterpret_cast<Il2CppObject*>(cn_name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    {
        auto skill_add_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "SkillAddMaster");
        auto skill_add_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ skill_add_class_wrapper->class_ptr_ });
        auto skill_add_master_data = Il2CppApi::RuntimeInvoke(skill_add_method, instance, &args, &exception);
        auto& dummy_skill = DummyClassBuilder::GetInstance().GetDummyClass(skill_add_master_data);
        auto list = dummy_skill.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto skill_add_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (skill_add_list->_size > 0) {
            LOGD("skill_add_list: %d", skill_add_list->_size);
            auto item = skill_add_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto name = dummy_element.GetString("name");

                    if (auto cn_name = Config::GetSkillName(name)) {
                        dummy_element.Set("name", cn_name);
                    }
                }
            }
        }
    }

    {
        auto buff_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "BuffMaster");
        auto buff_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ buff_class_wrapper->class_ptr_ });
        auto buff_master_data = Il2CppApi::RuntimeInvoke(buff_method, instance, &args, &exception);
        auto& dummy_buff = DummyClassBuilder::GetInstance().GetDummyClass(buff_master_data);
        auto list = dummy_buff.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto buff_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (buff_list->_size > 0) {
            LOGD("buff_list: %d", buff_list->_size);
            const auto& item = buff_list->_items;
            for (auto& element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto name = dummy_element.GetString("name");

                    if (auto cn_name = Config::GetBuffName(name)) {
                        dummy_element.Set("name", cn_name);
                    }

                    auto detail = dummy_element.GetString("detail");

                    if (auto cn_detail = Config::GetBuffDetail(detail)) {
                        dummy_element.Set("detail", cn_detail);
                    }
                }
            }
        }
    }

    {
        auto event_mission_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "EventMissionMaster");
        auto event_mission_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ event_mission_class_wrapper->class_ptr_ });
        auto event_mission_master_data = Il2CppApi::RuntimeInvoke(event_mission_method, instance, &args, &exception);
        auto& dummy_skill = DummyClassBuilder::GetInstance().GetDummyClass(event_mission_master_data);
        auto list = dummy_skill.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto event_mission_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (event_mission_list->_size > 0) {
            LOGD("event_mission_list: %d", event_mission_list->_size);
            auto item = event_mission_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto name = dummy_element.GetString("name");

                    if (auto cn_name = Config::GetMissionName(name)) {
                        dummy_element.Set("name", cn_name);
                    }

                    auto detail = dummy_element.GetString("detail");

                    if (auto cn_detail = Config::GetBuffDetail(detail)) {
                        dummy_element.Set("detail", cn_detail);
                    }
                }
            }
        }
    }

    {
        auto event_mission_condition_class_wrapper = Il2CppApi::Il2CppClassBuilder::GetInstance().GetClass("", "EventMissionConditionMaster");
        auto event_mission_condition_method = Il2CppApi::MakeGenericMethod((const MethodInfo*)target_method, std::vector<Il2CppClass*>{ event_mission_condition_class_wrapper->class_ptr_ });
        auto event_mission_condition_master_data = Il2CppApi::RuntimeInvoke(event_mission_condition_method, instance, &args, &exception);
        auto& dummy_skill = DummyClassBuilder::GetInstance().GetDummyClass(event_mission_condition_master_data);
        auto list = dummy_skill.Get<Il2CppObject*>("list");
        auto& dummy_observable_collection = DummyClassBuilder::GetInstance().GetDummyClass(list);
        auto event_mission_condition_list = dummy_observable_collection.Get<MyList<void*>*>("items");

        if (event_mission_condition_list->_size > 0) {
            LOGD("event_mission_condition_list: %d", event_mission_condition_list->_size);
            auto item = event_mission_condition_list->_items;
            for (auto element : *item) {
                if (element) {
                    auto& dummy_element = DummyClassBuilder::GetInstance().GetDummyClass(element);
                    auto name = dummy_element.GetString("conditionMessage");

                    if (auto cn_name = Config::GetMissionName(name)) {
                        dummy_element.Set("conditionMessage", cn_name);
                    }
                }
            }
        }
    }

    LOGI("Replaced MasterData");

    ori_CommonUI__InitMaskClick(_);
}

void Misc::Start() {
    AssetBundle::LoadFromMemoryAsync_Internal = (AssetBundle::LoadFromMemoryAsync_Internal_t)Il2CppApi::ResolveICall("UnityEngine.AssetBundle::LoadFromMemoryAsync_Internal(System.Byte[],System.UInt32)");
    AssetBundleCreateRequest::get_assetBundle = (AssetBundleCreateRequest::GetAssetBundle_t)Il2CppApi::ResolveICall("UnityEngine.AssetBundleCreateRequest::get_assetBundle()");
    AssetBundleRequest::get_allAssets = (AssetBundleRequest::GetAllAssets_t)Il2CppApi::ResolveICall("UnityEngine.AssetBundleRequest::get_allAssets()");
    AssetBundle::LoadAssetAsync_Internal = (AssetBundle::LoadAssetAsync_Internal_t)Il2CppApi::ResolveICall("UnityEngine.AssetBundle::LoadAssetAsync_Internal(System.String,System.Type)");

    info_list_.emplace_back("", "TitleRootComponent", "IsPlayedOpeningMovie", "", MakeInfo(TitleRootComponent_IsPlayedOpeningMovie));

    info_list_.emplace_back("", "CommonUI", "InitMaskClick", "", MakeInfo(CommonUI__InitMaskClick));

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
    else {
        is_loaded = true;
    }
}

void Misc::Hook() {
    HookTemplate::Hook();
    LoadInternationalization();
}