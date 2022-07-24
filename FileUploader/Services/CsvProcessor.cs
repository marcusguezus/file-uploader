using FileUploader.DAL.Command;
using FileUploader.Interface;
using FileUploader.Models;
using FileUploader.Models.Error;
using MediatR;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileUploader.Services
{
    public class CsvProcessor : IProcessor
    {
        private readonly IMediator _mediator;

        public CsvProcessor(IMediator mediator)
        {
            _mediator = mediator;

        }

        public ValidationResult ProcessFile(string fileContent)
        {

            var transactions = new List<Transaction>();
            var invalidInfo = new ValidationResult();

            Transaction transaction;

            var isFileValid = true;

            int line = 0;

            using (TextFieldParser parser = new TextFieldParser(new StringReader(fileContent)))
            {
                parser.CommentTokens = new string[] { "#" };
                parser.SetDelimiters(new string[] { "," });
                parser.HasFieldsEnclosedInQuotes = true;

                while (!parser.EndOfData)
                {


                    var readFields = parser.ReadFields();
                    if (readFields != null)
                    {
                        line += 1;
                        if (readFields.Length == 5)
                        {
                            transaction = new Transaction()
                            {
                                TransactionId = readFields[0],
                                Amount = Convert.ToDecimal(readFields[1]),
                                CurrencyCode = readFields[2],
                                TransactionDate = Convert.ToDateTime(readFields[3]),
                                Status = readFields[4]
                            };

                            transactions.Add(transaction);
                        }
                        else
                        {
                            invalidInfo.LineErrors.Add(line);
                            isFileValid = false;
                        }
                    }
                }
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
