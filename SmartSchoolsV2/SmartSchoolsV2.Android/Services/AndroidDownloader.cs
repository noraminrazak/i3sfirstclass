using Android.Content;
using Android.OS;
using SmartSchoolsV2.Droid.Services;
using SmartSchoolsV2.Services;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDownloader))]
namespace SmartSchoolsV2.Droid.Services
{
    public class AndroidDownloader : IDownloader
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;

        [Obsolete]
        public string DownloadFile(string url, string folder)
        {
            string pathToNewFile = string.Empty;
            string pathToNewFolder = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
            Directory.CreateDirectory(pathToNewFolder);

            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName(url));
                webClient.DownloadFileAsync(new Uri(url), pathToNewFile);

                Java.IO.File file = new Java.IO.File(pathToNewFile);

                Intent intent = new Intent(Intent.ActionView);
                string mimeType = string.Empty;

                if (Path.GetExtension(pathToNewFile).ToLower() == ".pdf")
                    mimeType = "application/pdf";
                else if (Path.GetExtension(pathToNewFile).ToLower() == ".doc")
                    mimeType = "application/msword";
                else if (Path.GetExtension(pathToNewFile).ToLower() == ".docx")
                    mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                else if (Path.GetExtension(pathToNewFile).ToLower() == ".xls")
                    mimeType = "application/vnd.ms-excel";
                else if (Path.GetExtension(pathToNewFile).ToLower() == ".xlsx")
                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                else if (Path.GetExtension(pathToNewFile).ToLower() == ".ppt")
                    mimeType = "application/vnd.ms-powerpoint";
                else if (Path.GetExtension(pathToNewFile).ToLower() == ".jpg")
                    mimeType = "image/jpeg";

                var t = Android.Net.Uri.FromFile(file);
                intent.SetDataAndType(t, mimeType);
                intent.SetFlags(ActivityFlags.GrantReadUriPermission);
                intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
                this.StartActivity(intent);

            }
            catch (Exception ex)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));

                //pathToNewFile = ex.Message;
            }

            return pathToNewFile;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }
            else
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(true));
            }
        }
    }

    public static class ObjectExtensions
    {
        public static void StartActivity(this object o, Intent intent)
        {
            var context = o as Context;
            if (context != null)
                context.StartActivity(intent);
            else
            {
                intent.SetFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(intent);
            }
        }
    }
}