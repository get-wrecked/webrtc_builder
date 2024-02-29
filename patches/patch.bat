git apply ..\..\webrtc_builder\patches\msvc2022.patch
xcopy ..\..\webrtc_builder\patches\config_components.h third_party\ffmpeg\config_components.h /Y /I /E
cd third_party\ffmpeg
git apply ..\..\..\..\webrtc_builder\patches\ffmpeg.patch