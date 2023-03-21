using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchoolsV2.Services
{
    public interface IDocumentViewer
    {
        void ShowDocumentFile(string filePath, string mimeType);
    }
}
