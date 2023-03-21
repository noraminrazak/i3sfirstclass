using Android.Content;
using SmartSchoolsV2.Droid.Services;
using SmartSchoolsV2.Services;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDocumentViewer))]
namespace SmartSchoolsV2.Droid.Services
{
    public class AndroidDocumentViewer : IDocumentViewer
    {
        public void ShowDocumentFile(string filepath, string mimeType)
        {
            var uri = Android.Net.Uri.Parse("file://" + filepath);
            var intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, mimeType);
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

            try
            {
                Forms.Context.StartActivity(Intent.CreateChooser(intent, "Select App"));
            }
            catch (Exception ex)
            {
                //Let the user know when something went wrong
            }
        }
    }
}