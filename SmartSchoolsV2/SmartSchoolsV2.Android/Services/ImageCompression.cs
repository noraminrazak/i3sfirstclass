using Android.Graphics;
using SmartSchoolsV2.Droid.Services;
using SmartSchoolsV2.Services;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageCompression))]
namespace SmartSchoolsV2.Droid.Services
{
    public class ImageCompression : IImageCompressionService
    {
        public ImageCompression() { }

        public byte[] CompressImage(byte[] imageData, string destinationPath, int compressionPercentage) {
            var resizedImage = GetResizedImage(imageData, compressionPercentage);
            var stream = new FileStream(destinationPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            stream.Write(resizedImage, 0, resizedImage.Length);
            stream.Flush();
            stream.Close();
            return resizedImage;
        }

        private byte[] GetResizedImage(byte[] imageData, int compressionPercentage) 
        {
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            using (MemoryStream ms = new MemoryStream())
            {
                originalImage.Compress(Bitmap.CompressFormat.Jpeg, compressionPercentage, ms);
                return ms.ToArray();
            }
        }
    }
}