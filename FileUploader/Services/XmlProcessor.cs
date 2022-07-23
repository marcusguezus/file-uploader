using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileUploader.Models;
using System.Xml;
using System.Xml.Linq;
using MediatR;

namespace FileUploader.Services
{
    public class XmlProcessor : IProcessor
    {
        private readonly IMediator _mediator;
        public XmlProcessor(IMediator mediator)
        {
            _mediator = mediator;

        }
        public bool ProcessFile(string fileContent)
        {
            var xmlDoc = XDocument.Parse(fileContent);

            var items = (from element in xmlDoc.Root.Elements("Transaction")
                         select new Transaction
                         {
                             TransactionId = element.Attribute("id").Value,
                             TransactionDate = Convert.ToDateTime(element.Element("TransactionDate").Value),
                             Amount = Convert.ToDecimal(element.Element("PaymentDetails").Element("Amount").Value),
                             CurrencyCode = element.Element("PaymentDetails").Element("CurrencyCode").Value,
                             Status = element.Element("Status").Value
                         }).ToList();

            //_mediator

            return true;
        }
    }
}
