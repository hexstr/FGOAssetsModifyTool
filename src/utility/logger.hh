#ifndef LOGGER_HEADER
#define LOGGER_HEADER

#ifndef LOG_TAG
#define LOG_TAG "hexstr"
#include "android/log.h"

#ifndef NDEBUG
#ifndef ENCRYPTION
#define LOGD(...) __android_log_print(ANDROID_LOG_DEBUG, LOG_TAG, __VA_ARGS__)
#endif
#else
#define LOGD(...)
#endif

#define LOGI(...) __android_log_print(ANDROID_LOG_INFO, LOG_TAG, __VA_ARGS__)
#define ERROR(...) __android_log_print(ANDROID_LOG_ERROR, LOG_TAG, __VA_ARGS__)
#endif

#endif // LOGGER_HEADER