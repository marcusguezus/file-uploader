using FileUploader.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploader.DAL.Command
{
    public class CreateTransactionsCommand : IRequest
    {
        public List<Transaction> Transactions { get; set; }

        public class CommandHandler : IRequestHandler<CreateTransactionsCommand>
        {
            private readonly TransactionContext _dbContext;


            public CommandHandler(TransactionContext dbContext)
            {
                _dbContext = dbContext;
            }

            public Task<Unit> Handle(CreateTransactionsCommand command, CancellationToken cancellationToken)
            {
                _dbContext.Transactions.AddRange(command.Transactions);
                _dbContext.SaveChanges();

                return Task.FromResult(Unit.Value);
            }
        }
    }
}
