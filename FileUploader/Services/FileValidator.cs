using FileUploader.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploader.Services
{
    public class FileValidator : IFileValidator
    {
        public bool ValidateFile(string fileName, long fileLength, out string message)
        {
            bool pass = true;
            message = "";
            var fileExtension = fileName.Split('.')[1];
            if (fileExtension != "csv" && fileExtension != "xml")
            {
                pass = false;
                message = "Unknown Format";
            }
            else if (ConvertBytesToMegabytes(fileLength) > 1f) {
                pass = false;
                message = "File must not be more than 1MB";
            }

            return pass;
        }

        double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }
}
