using FileUploader.Models.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploader.Interface
{
    public interface IProcessor
    {
        ValidationResult ProcessFile(string fileContent);
    }
}
