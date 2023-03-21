using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchoolsV2.Services
{
    public interface IImageCompressionService
    {
        byte[] CompressImage(byte[] imageData, string destinationPath, int compressionPercentage);
    }
}
