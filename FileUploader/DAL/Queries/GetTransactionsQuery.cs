using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileUploader.Models;
using System.Threading;

namespace FileUploader.DAL.Queries
{
    public class GetTransactionsQuery : IRequest<List<Transaction>>
    {

        public class QueryHandler : IRequestHandler<GetTransactionsQuery, List<Transaction>>
        {
            private readonly TransactionContext _dbContext;

            public QueryHandler(TransactionContext dbContext)
            {
                _dbContext = dbContext;
            }

            public Task<List<Transaction>> Handle(GetTransactionsQuery query, CancellationToken cancellationToken)
            {
                var transactionList = _dbContext.Transactions.ToList();

                return Task.FromResult(transactionList);
            }
        }
    }
  
}
