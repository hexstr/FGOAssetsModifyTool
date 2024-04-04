// ModFGO.cpp: 定义应用程序的入口点。
//

#include <dlfcn.h>
#include <unistd.h>

#include <filesystem>
#include <thread>

#include <il2cpp_api.hh>
#include <my_exception.hh>

#include "Hook/Launch.hh"
#include "utility/logger.hh"

#ifndef __x86_64__

void StartHookProc() {
    std::thread([=]() {
        sleep(10);
        try {
            Il2CppApi::Il2CppLoader::GetInstance().Initialize("Assembly-CSharp.dll",
                                                              nullptr);
            Launch::Start();
        } catch (std::exception& ex) {
            ERROR("std::exception %s", ex.what());
        }
    }).detach();
}

#define INIT_LOAD

#endif

#ifdef INIT_LOAD

[[gnu::constructor]] void init() {
    StartHookProc();
}

#else

#include "FGOAssetReplace.hh"

REGISTER_ZYGISK_MODULE(MyModule)

void MyModule::onLoad(Api* api, JNIEnv* env) {
    this->api = api;
    this->env = env;
}

void MyModule::preAppSpecialize(AppSpecializeArgs* args) {
    const char* process = env->GetStringUTFChars(args->nice_name, nullptr);
    preSpecialize(process);
    env->ReleaseStringUTFChars(args->nice_name, process);
}

void MyModule::postAppSpecialize(const AppSpecializeArgs* args) {
    if (is_target_) {
#if defined(__aarch64__)
        StartHookProc();
#elif defined(__x86_64__)

        std::thread([=]() {
            sleep(5);
            void* handle = dlopen("/system/lib64/libnativebridge.so", RTLD_NOW);

            using NativeBridgeLoadLibraryExt_t = void* (*)(const char*, int, int);
            static NativeBridgeLoadLibraryExt_t NativeBridgeLoadLibraryExt = nullptr;
            NativeBridgeLoadLibraryExt = (NativeBridgeLoadLibraryExt_t)dlsym(
                handle, "_ZN7android26NativeBridgeLoadLibraryExtEPKciPNS_25native_"
                        "bridge_namespace_tE");

            using NativeBridgeGetTrampoline_t =
                void* (*)(void* handle, const char* name, const char* shorty,
                          uint32_t len);
            static NativeBridgeGetTrampoline_t NativeBridgeGetTrampoline = nullptr;
            NativeBridgeGetTrampoline = (NativeBridgeGetTrampoline_t)dlsym(
                handle, "_ZN7android25NativeBridgeGetTrampolineEPvPKcS2_j");

            using NativeBridgeGetError_t = const char* (*)(void);
            static NativeBridgeGetError_t NativeBridgeGetError = nullptr;
            NativeBridgeGetError = (NativeBridgeGetError_t)dlsym(
                handle, "_ZN7android20NativeBridgeGetErrorEv");

            void* result = NativeBridgeLoadLibraryExt(
                "/data/local/tmp/gh@hexstr/FGOAssetReplace/libBGOAssetReplace.so",
                RTLD_NOW, 3);
            if (result) {
            }
            else {
                ERROR("Load plugin failed. %s", NativeBridgeGetError());
            }
        }).detach();
#endif
    }
}

void MyModule::preSpecialize(const char* process) {
    if (std::strstr(process, "fatego") || std::strstr(process, "bilibili.fgo") ||
        std::strstr(process, "com.tencent.tmgp.fgo")) {
        is_target_ = true;
    }
}

#endif