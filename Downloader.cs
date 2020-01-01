using System;
using System.Collections.Generic;
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

        public async Task<bool> Download()
        {
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
                        $"VideoDownloaded{DateTime.UtcNow:yyyyMMDDhhmm}.{ext}", this.Displayprogress());
                }
                else
                {
                    await client.DownloadMediaStreamAsync(streamInfo, $"AudioDownloaded{DateTime.UtcNow:yyyyMMDDhhmm}.{ext}", this.Displayprogress());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        private IProgress<double> Displayprogress()
        {
           return new Progress<double>(p =>
           {
               Console.SetCursorPosition(0, 6);
               Console.Write(Convert.ToInt16(p*100).ToString() + "% \r");
           });
        }
    }
}
