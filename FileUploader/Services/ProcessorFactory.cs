using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploader.Services
{
    public static class ProcessorFactory
    {

        public static IProcessor GetProcessorInstance(string fileExtension, IMediator mediator)
        {
            IProcessor processor = null;
            switch (fileExtension)
            {
                case "csv":
                    processor = new CsvProcessor(mediator);
                    break;
                case "xml":
                    processor = new XmlProcessor(mediator);
                    break;
            }

            return processor;
        }
    }
}
