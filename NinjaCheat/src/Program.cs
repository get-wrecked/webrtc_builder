using System;
using System.IO;
using System.Text.RegularExpressions;

namespace NinjaCheat
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1 || string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("Define a ninja output directory to cheat");
                return;
            }

            string workDir = args[0];
            if (!Directory.Exists(workDir))
            {
                // try to use the specified path as relative
                string currentDir = Directory.GetCurrentDirectory();
                workDir = Path.Combine(currentDir, workDir);

                if (!Directory.Exists(workDir))
                {
                    Console.WriteLine("The specified directory does not exist!");
                    return;
                }
            }

            HackFilesInDirectory(workDir);

            string libyuvPath = Path.Combine(workDir, @"win_clang_x64\obj\third_party\libyuv\libyuv_internal.ninja");
            HackLibyuv(libyuvPath);
            
            string pffftPath = Path.Combine(workDir, @"obj\third_party\pffft\pffft.ninja");
            HackPffft(pffftPath);

            string ffmpegPath = Path.Combine(workDir, @"obj\third_party\ffmpeg\ffmpeg_internal.ninja");
            HackFfmpeg(ffmpegPath);
        }

        static int filesHacked = 0;
        static void HackFilesInDirectory(string dir)
        {
            var dirs = Directory.EnumerateDirectories(dir);
            foreach (string innerDir in dirs)
                HackFilesInDirectory(innerDir);

            var files = Directory.GetFiles(dir, "*.ninja");
            foreach (string file in files)
            {
                string text = File.ReadAllText(file);
                Console.WriteLine("Hacking file (" + filesHacked++ + "): " + file);
                if (text.Contains(" /MTd"))
                    text = text.Replace("/MTd", "/MDd");
                else
                    text = text.Replace("/MT", "/MD");
                File.WriteAllText(file, text);
            }
        }

        static void HackLibyuv(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("libyuv_internal.ninja file wasn't found in the provided folder! Make sure the path is correct");
                return;
            }

            string text = File.ReadAllText(filePath);
            Console.WriteLine("Hacking libyuv file (" + filesHacked++ + "): " + filePath);
            text = text.Replace(" /llvmlibthin", "");
            File.WriteAllText(filePath, text);
        }

        static void HackPffft(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("pffft.ninja file wasn't found in the provided folder! Make sure the path is correct");
                return;
            }

            string text = File.ReadAllText(filePath);
            Console.WriteLine("Hacking pffft file (" + filesHacked++ + "): " + filePath);
            text = text.Replace(" -Wno-shadow", "");
            File.WriteAllText(filePath, text);
        }

        static void HackFfmpeg(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("ffmpeg_internal.ninja file wasn't found in the provided folder! Make sure the path is correct");
                return;
            }

            string text = File.ReadAllText(filePath);
            Console.WriteLine("Hacking ffmpeg file (" + filesHacked++ + "): " + filePath);
            text = text.Replace("source_name_part = bitstream\n",
                "source_name_part = bitstream\n" +
                "build obj/third_party/ffmpeg/ffmpeg_internal/null_bsf.obj: cc ../../third_party/ffmpeg/libavcodec/null_bsf.c || obj/third_party/ffmpeg/ffmpeg_features.stamp\n" +
                "  source_file_part = null_bsf.c\n" +
                "  source_name_part = null_bsf\n");
            text = text.Replace("obj/third_party/ffmpeg/ffmpeg_internal/bitstream.obj ",
                "obj/third_party/ffmpeg/ffmpeg_internal/bitstream.obj obj/third_party/ffmpeg/ffmpeg_internal/null_bsf.obj ");
            File.WriteAllText(filePath, text);
        }
    }
}
