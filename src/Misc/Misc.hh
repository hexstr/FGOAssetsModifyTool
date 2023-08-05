#ifndef MISC_HEADER
#define MISC_HEADER

#include "Hook/HookTemplate.hh"

class AssetBundleRequest;
class AssetBundleCreateRequest;

class AssetBundle {
    friend class Misc;

public:
    static AssetBundleCreateRequest* LoadFromMemoryAsync(void* binary) {
        return LoadFromMemoryAsync_Internal(binary, 0);
    }
    AssetBundleRequest* LoadAssetAsync(Il2CppString* name, Il2CppReflectionType* type) {
        return LoadAssetAsync_Internal(this, name, type);
    }

private:
    using LoadFromMemoryAsync_Internal_t = AssetBundleCreateRequest* (*)(void*, int);
    static LoadFromMemoryAsync_Internal_t LoadFromMemoryAsync_Internal;

    using LoadAssetAsync_Internal_t = AssetBundleRequest* (*)(AssetBundle*, Il2CppString*, Il2CppReflectionType*);
    static LoadAssetAsync_Internal_t LoadAssetAsync_Internal;

    intptr_t m_CachedPtr;
};

class AssetBundleRequest {
    friend class Misc;

public:
    MyArray<AssetBundle*>* GetAllAssets() {
        return get_allAssets(this);
    }

private:
    using GetAllAssets_t = MyArray<AssetBundle*>* (*)(AssetBundleRequest*);
    static GetAllAssets_t get_allAssets;
};

class AssetBundleCreateRequest {
    friend class Misc;

public:
    AssetBundle* GetAssetBundle() {
        return get_assetBundle(this);
    }

private:
    using GetAssetBundle_t = AssetBundle* (*)(AssetBundleCreateRequest*);
    static GetAssetBundle_t get_assetBundle;
};

class Misc : public HookTemplate {
public:
    virtual void Start() override final;
    virtual void Hook() override final;
    void LoadInternationalization();
};

#endif // Misc.hh