using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploader.Interface
{
    public interface IFileValidator
    {
        bool ValidateFile(string fileName, long fileLength, out string message);
    }
}
