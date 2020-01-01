using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;

namespace YoutubeVideoDownloader
{
    public class Downloader :IDownloader
    {
        private string url;
        private bool audioOnly;

        public Downloader(string url, bool audioOnly)
        {
            this.url = url;
            this.audioOnly = audioOnly;
        }

        public async Task<DownloadResult> Download()
        {
            var result = new DownloadResult();
            var id = YoutubeClient.ParseVideoId(this.url);

            var client = new YoutubeClient();

            var streamInfoSet = await client.GetVideoMediaStreamInfosAsync(id);
            MediaStreamInfo streamInfo;
            if (!audioOnly)
            {
                streamInfo = streamInfoSet.Muxed.WithHighestVideoQuality();
            }
            else
            {
                streamInfo = streamInfoSet.Audio.WithHighestBitrate();
            }

          

            var ext = streamInfo.Container.GetFileExtension();
            try
            {
                if (!this.audioOnly)
                {

                    await client.DownloadMediaStreamAsync(streamInfo,
                        $"VideoDownloaded{DateTime.UtcNow:yyyyMMddhhmmss}.{ext}", this.Displayprogress());
                    result.Successful = true;
              
                }
                else
                {
                    await client.DownloadMediaStreamAsync(streamInfo, $"AudioDownloaded{DateTime.UtcNow:yyyyMMddhhmmss}.{ext}", this.Displayprogress());
                    result.Successful = true;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Successful = false;
                
            }
            result.OutputFolder = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            return result;
        }

        private IProgress<double> Displayprogress()
        {
           return new Progress<double>(p =>
           {
               Console.SetCursorPosition(0, 5);
               Console.Write(Convert.ToInt16(p*100).ToString() + "% \r");
           });
        }
    }

    public class DownloadResult
    {
        public bool Successful { get; set; }

        public string OutputFolder { get; set; }

    }
}
