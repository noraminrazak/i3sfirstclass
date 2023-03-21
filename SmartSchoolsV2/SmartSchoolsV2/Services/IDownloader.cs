using System;

namespace SmartSchoolsV2.Services
{
    public interface IDownloader
    {
        string DownloadFile(string url, string folder);
        event EventHandler<DownloadEventArgs> OnFileDownloaded;
    }

    public class DownloadEventArgs : EventArgs
    {
        public bool FileSaved = false;
        public DownloadEventArgs(bool fileSaved)
        {
            FileSaved = fileSaved;
        }
    }
}
