using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileUploader.Models;
using System.Xml;
using System.Xml.Linq;
using MediatR;
using FileUploader.DAL.Command;

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

            var transactions = new List<Transaction>();

            var isFileValid = true;

            foreach (var element in xmlDoc.Root.Elements("Transaction"))
            {
                var transaction = new Transaction();

                var transactionId = element.Attribute("id").Value;
                if (!String.IsNullOrEmpty(transactionId))
                {
                    transaction.TransactionId = transactionId;
                }
                else
                {
                    isFileValid = false;
                    continue;
                }

                var tansactionDate = Convert.ToDateTime(element.Element("TransactionDate").Value);
                if (tansactionDate != DateTime.MinValue)
                {
                    transaction.TransactionDate = tansactionDate;
                }
                else
                {
                    isFileValid = false;
                    continue;
                }

                var amount = Convert.ToDecimal(element.Element("PaymentDetails").Element("Amount").Value);
                if (amount != default(decimal))
                {
                    transaction.Amount = amount;
                }
                else
                {
                    isFileValid = false;
                    continue;
                }

                var currencyCode = element.Element("PaymentDetails").Element("CurrencyCode").Value;
                if (!String.IsNullOrEmpty(currencyCode))
                {
                    transaction.CurrencyCode = currencyCode;
                }
                else
                {
                    isFileValid = false;
                    continue;
                }

                var status = element.Element("Status").Value;
                if (!String.IsNullOrEmpty(status))
                {
                    transaction.Status = status;
                }
                else
                {
                    isFileValid = false;
                    continue;
                }

                transactions.Add(transaction);
            }

            if (isFileValid && transactions.Any())
            {
                var createTransactionsCommand = new CreateTransactionsCommand()
                {
                    Transactions = transactions
                };

                _ = _mediator.Send(createTransactionsCommand).Result;
            }
            return isFileValid;
        }
    }
}
