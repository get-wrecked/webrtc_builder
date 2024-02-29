Google WebRTC

This is a clone of [Google WebRTC repositiry](https://webrtc.googlesource.com/src) of **M121 release (01/23/24)** for internal purposes.

---

## Content

The repository contains:
- The minimal set of headers used for WebRTC streaming purposes in Scope (*include* directory)
- Static libraries, built using **MSVC 14.37.32822** (Visual Studio 2022) and **Windows SDK 10.0.20348.0** (*library* directory)
- All the flags the static libraries were built with
- Tools and patches to rebuild the static libraries if needed

## Build process

#### Debug
1. Get [Depot Tools](https://storage.googleapis.com/chrome-infra/depot_tools.zip) and unpack them (e.g. C:\depot_tools)

2. Run Command Prompt as admin

3. Add Depot Tools to your the environment PATH:
`set PATH=C:\depot_tools;%PATH%`

4. Configure Depot Tools:
`cd C:\depot_tools`
`gclient`

5. Define other environment variables:
`set DEPOT_TOOLS_WIN_TOOLCHAIN=0`
`set GYP_MSVS_VERSION=2022`

6. Create a new directory for WebRTC:
`cd C:\your_target_dir`
`mkdir webrtc`
`cd webrtc`

7. Pull the latest sources of WebRTC (this can take about 30 min):
`fetch --nohooks webrtc`

8. Check out the latest stable branch (M121 release of Januray, 23rd, 2024):
`cd src`
`git checkout branch-heads/6167`

9. Update Depot Tools to target the selected release (this can take about 20 min):
`cd ..`
`gclient sync`

10. Apply patches to make the sources buildable in VS 2022:
`"..\..\webrtc_builder\patches\patch.bat"`

11. Run a command to generate Ninja build files and configs:
`gn gen --ide=vs2022 out/Debug --args="is_debug=true ffmpeg_branding=\"Chromium\" rtc_use_h264=true enable_iterator_debugging=true use_rtti=true is_clang=false rtc_build_tools=false rtc_include_tests=false rtc_build_examples=false windows_sdk_version=\"10.0.20348.0\""`

12. Run Ninja to build all the Debug libraries:
`ninja -C out/Debug`

13. When you face this link error:
`FAILED: obj/third_party/pffft/pffft/pffft.obj`
`cl : Command line error D8021 : invalid numeric argument '/Wno-shadow'`
Run the following command
`..\..\webrtc_builder\NinjaCheat\NinjaCheat out\Debug`

#### Release
The same build steps from 1 to 10 of the Debug build are also applied for Release. Other steps are a bit different:

11. Run a command to generate Ninja build files and configs:
`gn gen --ide=vs2022 out/Release --args="is_debug=false ffmpeg_branding=\"Chromium\" rtc_use_h264=true enable_iterator_debugging=true use_rtti=true is_clang=false rtc_build_tools=false rtc_include_tests=false rtc_build_examples=false windows_sdk_version=\"10.0.20348.0\""`

12. Run Ninja to build all the Release libraries:
`ninja -C out/Release`

13. When you face this link error:
`FAILED: obj/third_party/pffft/pffft/pffft.obj`
`cl : Command line error D8021 : invalid numeric argument '/Wno-shadow'`
Run the following command
`scope_dir\tools\webrtc\NinjaCheat\NinjaCheat out\Release`

---

## Troubleshooting

Errors you can see during the build process and how to resolve them

- Here are the errors you could see if you haven't applied the VS 2022 patch (see step 10 above) or the
patch hasn't been applied successfully

#### ../../api/test/metrics/metrics_accumulator.cc(X): error C7555: use of designated initializers requires at least '/std:c++20'

#### D:\Code\webrtc-clang\src\api\test\metrics\metrics_logger.cc(57) : fatal error C1001: Internal compiler error.

#### ../../modules/audio_device/win/audio_device_core_win.cc(X): error C2362: initialization of 'X' is skipped by 'goto Exit'

#### ../../modules/audio_processing/aec3/matched_filter_avx2.cc(84): error C2676: binary '': '__m256' does not define this operator or a conversion to a type acceptable to the predefined operator

#### ../../pc/legacy_stats_collector.cc(191): warning C4838: conversion from 'const double' to 'const ValueType &' requires a narrowing conversion

####../../third_party/ffmpeg/libavcodec/allcodecs.c(924): error C2039: 'init_static_data': is not a member of 'AVCodec'
#### ..\src\third_party\ffmpeg\libavcodec\codec.h(187): note: see declaration of 'AVCodec'
---

## Further Help
These forum topics helped me along the way of building WebRTC for MSVC:
https://groups.google.com/g/discuss-webrtc/c/PBJK6LMzNmU
https://groups.google.com/g/discuss-webrtc/c/cMeYxhpB86k

Extended installation guide by Google:
https://chromium.googlesource.com/chromium/src/+/master/docs/windows_build_instructions.md