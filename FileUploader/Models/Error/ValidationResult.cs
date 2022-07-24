using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploader.Models.Error
{
    public class ValidationResult
    {
        public bool IsFileValid { get; set; }
        public List<int> LineErrors { get; set; } = new List<int>();
    }
}
