using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeVideoDownloader
{
    interface IDownloader
    {
        Task<bool> Download();
    }
}
