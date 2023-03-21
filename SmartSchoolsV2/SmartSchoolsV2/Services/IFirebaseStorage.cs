using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchoolsV2.Services
{
    public interface IFirebaseStorage
    {
        Task<string> UploadFiles();

        Task<string> GetFileUrl(string filename);
    }
}
