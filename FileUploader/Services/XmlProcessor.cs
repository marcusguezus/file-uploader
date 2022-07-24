using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileUploader.Models;
using System.Xml;
using System.Xml.Linq;
using MediatR;
using FileUploader.DAL.Command;
using FileUploader.Models.Error;
using FileUploader.Interface;

namespace FileUploader.Services
{
    public class XmlProcessor : IProcessor
    {
        private readonly IMediator _mediator;
        public XmlProcessor(IMediator mediator)
        {
            _mediator = mediator;

        }
        public ValidationResult ProcessFile(string fileContent)
        {
            var xmlDoc = XDocument.Parse(fileContent);
            var invalidInfo = new ValidationResult();
            var transactions = new List<Transaction>();
            var line = 0;
            var isFileValid = true;

            foreach (var element in xmlDoc.Root.Elements("Transaction"))
            {
                var transaction = new Transaction();
                line += 1;
                var transactionId = element.Attribute("id").Value;
                if (!String.IsNullOrEmpty(transactionId))
                {
                    transaction.TransactionId = transactionId;
                }
                else
                {
                    isFileValid = false;
                    invalidInfo.LineErrors.Add(line);
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
                    invalidInfo.LineErrors.Add(line);
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
                    invalidInfo.LineErrors.Add(line);
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
                    invalidInfo.LineErrors.Add(line);
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
                    invalidInfo.LineErrors.Add(line);
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

            invalidInfo.IsFileValid = isFileValid;

            return invalidInfo;
        }
    }
}
