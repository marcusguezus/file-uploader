using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploader.DataTransferObject
{
    public class TransactionDto
    {
        public string Id { get; set; }
        public string Payment { get; set; }
        public string Status { get; set; }
    }
}
