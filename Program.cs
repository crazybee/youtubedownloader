using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace YoutubeVideoDownloader
{
    internal class Program
    {
        static async Task Main(string[] args)

        {

            var continueDownload = "y";
            var downloadResult = new DownloadResult();

            while (string.Equals(continueDownload, "y", StringComparison.InvariantCultureIgnoreCase))
            {
                downloadResult = await DownloadAction();
                do
                {
                    Console.WriteLine("download another one?(y/n)");
                    continueDownload = Console.ReadLine();

                } while (!string.Equals(continueDownload, "y", StringComparison.InvariantCultureIgnoreCase) &&
                         !string.Equals(continueDownload, "n", StringComparison.InvariantCultureIgnoreCase));

            }

            if (string.Equals(continueDownload, "n", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Thanks bye bye. ");
                OpenFolder(downloadResult.OutputFolder);

            }
        }

        private static async Task<DownloadResult> DownloadAction()
        {
            string url;
            Console.Clear();
            do
            {
                Console.WriteLine("paste youtube url here");
                url = Console.ReadLine();
            } while (url == string.Empty);

            string audioOnly;
            do
            {
                Console.WriteLine("only extract audio?(y/n)");
                audioOnly = Console.ReadLine();
            } while (audioOnly != "y" && audioOnly != "n");

            var downloader = new Downloader(url, string.Equals(audioOnly, "y", StringComparison.InvariantCultureIgnoreCase));
            Console.WriteLine("downloading...");
            var downloadResult = await downloader.Download();
            Console.WriteLine(downloadResult.Successful ? "Downloaded" : "Failed");


            return downloadResult;
        }

        private static void OpenFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
            else
            {
                Console.WriteLine("Folder doesn't exists ");
            }
        }

    }

}
