using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileUploader.Models;
using System.Threading;
using FileUploader.DataTransferObject;

namespace FileUploader.DAL.Queries
{
    public class GetTransactionsQuery : IRequest<IQueryable<Transaction>>
    {

        public class QueryHandler : IRequestHandler<GetTransactionsQuery, IQueryable<Transaction>>
        {
            private readonly TransactionContext _dbContext;

            public QueryHandler(TransactionContext dbContext)
            {
                _dbContext = dbContext;
            }

            public Task<IQueryable<Transaction>> Handle(GetTransactionsQuery query, CancellationToken cancellationToken)
            {
                var transactionList = _dbContext.Transactions.AsQueryable();

                return Task.FromResult(transactionList);
            }
        }
    }
  
}
