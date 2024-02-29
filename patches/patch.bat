git apply ..\..\webrtc_builder\patches\msvc2022.patch
xcopy /Y /E ..\..\webrtc_builder\patches\config_components.h third_party\ffmpeg\config_components.h
cd third_party\ffmpeg
git apply ..\..\..\..\webrtc_builder\patches\ffmpeg.patch