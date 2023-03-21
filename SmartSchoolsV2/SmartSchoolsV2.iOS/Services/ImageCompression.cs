using Foundation;
using SmartSchoolsV2.iOS.Services;
using SmartSchoolsV2.Services;
using System;
using System.IO;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageCompression))]
namespace SmartSchoolsV2.iOS.Services
{
    public class ImageCompression : IImageCompressionService
    {
        public ImageCompression() {}

        public byte[] CompressImage(byte[] imageData, string destinationPath, int compressionPercentage) 
        {
            UIImage originalImage = ImageFromByteArray(imageData);

            if (originalImage != null) {
                nfloat compressionQuality = (nfloat)(compressionPercentage / 100.0);
                var resizedImage = originalImage.AsJPEG(compressionQuality).ToArray();
                var stream = new FileStream(destinationPath, FileMode.Create);
                stream.Write(resizedImage, 0, resizedImage.Length);
                stream.Flush();
                stream.Close();

                return resizedImage;
            }

            return imageData;
        }

        public static UIImage ImageFromByteArray(byte[] data) 
        {
            if (data == null) return null;

            UIImage image;

            try 
            {
                image = new UIImage(NSData.FromArray(data));
            }
            catch (Exception e) 
            {
                //new Logger().LogError(new Exception("PhotoProcessingService", e));
                return null;
            }

            return image;
        }
    }
}