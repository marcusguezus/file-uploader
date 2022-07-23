using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploader
{
    public interface IProcessor
    {
        bool ProcessFile(string fileContent);
    }
}
