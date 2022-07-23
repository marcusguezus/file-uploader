using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploader.Models
{
    public class FileValidation
    {
        public int MaxFileSizeInBytes { get; }

        public List<string> ValidExtensions { get; }
    }
}
