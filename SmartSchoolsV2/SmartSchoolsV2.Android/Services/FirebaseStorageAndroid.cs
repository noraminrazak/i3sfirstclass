using System;
using System.IO;
using System.Threading.Tasks;
using SmartSchoolsV2.Droid.Services;
using SmartSchoolsV2.Services;
using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.Gms.Tasks;
using Firebase.Storage;
using Xamarin.Forms;
using Object = Java.Lang.Object;

[assembly: Dependency(typeof(FirebaseStorageAndroid))]
namespace SmartSchoolsV2.Droid.Services
{
    public class FirebaseStorageAndroid : IFirebaseStorage
    {
        private string _storage = "gs://i3sv2-1f5dd.appspot.com/";

        /// <summary>
        /// Upload local file to external storage
        /// </summary>
        /// <returns>Url to external file</returns>
        public async Task<string> UploadFiles()
        {
            var result = "";
            int timeWait = 250;
            int countWait = 0;
            int maxCountWait = 20;

            try
            {
                var activity = Forms.Context as Activity;

                PickFileActivity.OnFinishAction = async (path) =>
                {
                    result = await SaveFileToStorage(path);
                };

                activity?.StartActivity(new Intent(Forms.Context, typeof(PickFileActivity)));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("---> Error UploadFiles " + ex.Message);
                countWait = maxCountWait;
            }

            do
            {
                countWait++;
                await System.Threading.Tasks.Task.Delay(timeWait);
            }
            while (result == "" || countWait < maxCountWait);

            return result;
        }

        /// <summary>
        /// Get Url to external(firebase storage) file 
        /// </summary>
        /// <param name="filename">name file</param>
        /// <returns>Url to external file</returns>
        public async Task<string> GetFileUrl(string filename)
        {
            try
            {
                var storage = FirebaseStorage.Instance;
                var storageRef = storage.GetReferenceFromUrl(_storage);
                var spaceRef = storageRef.Child($"images/{filename}");
                var url = await spaceRef.GetDownloadUrl();

                filename = url.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("---> Error UploadFiles " + ex.Message);
            }

            return filename;
        }

        #region Helpers

        /// <summary>
        ///  Save File To Firebase Storage and return url to file
        /// </summary>
        /// <param name="localPath">url to local file</param>
        /// <returns>url to external file</returns>
        private async Task<string> SaveFileToStorage(string localPath)
        {
            try
            {
                var storage = FirebaseStorage.Instance;
                var storageRef = storage.GetReferenceFromUrl("gs://i3sv2-1f5dd.appspot.com/");

                var bytes = System.IO.File.ReadAllBytes(localPath);
                var metadata = new StorageMetadata.Builder()
                    .SetContentType("image/jpeg")
                    .Build();

                var child = storageRef.Child("images/" + Path.GetFileName(localPath));
                await child.PutBytes(bytes, metadata);

                localPath = Path.GetFileName(localPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("---> Error SaveFileToStorage " + ex.Message);
            }

            return localPath;
        }

        #endregion
    }
}