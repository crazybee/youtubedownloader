using System;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;

namespace YoutubeVideoDownloader
{
    class Program
    {
        static async Task Main(string[] args)

        {
             string url;
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
            Console.WriteLine(downloadResult ? "Downloaded" : "Failed");
        }
    }
}
