using FileUploader.DAL.Queries;
using FileUploader.DataTransferObject;
using FileUploader.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        private readonly IMediator _mediator;
        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("currency/{currency}")]
        public async Task<IActionResult> GetTransactionsByCurrency(string currency)
        {
            var query = new GetTransactionsQuery();
            var transactionList = await _mediator.Send(query);

            var result = from tran in transactionList
                         where tran.CurrencyCode == currency.ToUpper()
                         select new TransactionDto
                         {
                             Id = tran.TransactionId,
                             Payment = $"{tran.Amount} {tran.CurrencyCode}",
                             Status = EvaluateStatus(tran.Status)
                         };

            return Ok(result);
        }

        [HttpGet("date/{from}&{to}")]
        public async Task<IActionResult> GetTransactionsByDate(string from, string to)
        {
            var startDate = Convert.ToDateTime(from);
            var endDate = Convert.ToDateTime(to);

            var query = new GetTransactionsQuery();
            var transactionList = await _mediator.Send(query);

            var result = from tran in transactionList
                         where tran.TransactionDate.Date >= startDate && tran.TransactionDate.Date <= endDate
                         select new TransactionDto
                         {
                             Id = tran.TransactionId,
                             Payment = $"{tran.Amount} {tran.CurrencyCode}",
                             Status = EvaluateStatus(tran.Status)
                         };

            return Ok(result);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetTransactionsByStatus(string status)
        {
            var query = new GetTransactionsQuery();
            var transactionList = await _mediator.Send(query);

            var result = from tran in transactionList
                         where tran.Status == status
                         select new TransactionDto
                         {
                             Id = tran.TransactionId,
                             Payment = $"{tran.Amount} {tran.CurrencyCode}",
                             Status = EvaluateStatus(tran.Status)
                         };

            return Ok(result);
        }


        private string EvaluateStatus(string status)
        {
            string result = "N/A";
            switch (status)
            {
                case StatusTypes.Approved:
                    result = "A";
                    break;
                case string s when (s == StatusTypes.Failed || s == StatusTypes.Rejected):
                    result = "R";
                    break;
                case string s when (s == StatusTypes.Finished || s == StatusTypes.Done):
                    result = "D";
                    break;

            }

            return result;
        }
    }
}
