using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using System.Linq;
using OpenChart.Application.Common;
using OpenChart.Application.Services;
using OpenChart.Domain.Entities;

namespace OpenChart.Application.Queries
{
    public class GetTsvCandlesHandler : IRequestHandler<GetTsvCandlesQuery, string>
    {
        private readonly IDbContext _dbContext;

        public GetTsvCandlesHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> Handle(GetTsvCandlesQuery request, CancellationToken cancellationToken)
        {
            //todo: get exchange data from db

            var exchange = new Exchange
            {
                ClassCode = request.ClassCode,
                TradeStart = (long) TimeSpan.FromHours(7).TotalMilliseconds
            };

            var candlesResponse = await _dbContext.GetCollection(request.ClassCode, request.SecurityCode, DbType(request.TimeFrame))
                .AsQueryable()
                .Where(x => x.Date > request.StartDate && x.Date < request.EndDate)
                .ToAsyncEnumerable()
                .ChangeTimeFrame(exchange, request.SecurityCode, request.TimeFrame, cancellationToken)
                .WritToTsv(cancellationToken: cancellationToken);

            return candlesResponse;
        }

        private DataBaseType DbType(TimeFrame timeFrame) => timeFrame switch
        {
            TimeFrame.H1 => DataBaseType.Hour,
            TimeFrame.H4 => DataBaseType.Hour,
            TimeFrame.D => DataBaseType.Day,
            TimeFrame.W => DataBaseType.Day,
            TimeFrame.MN => DataBaseType.Day,
            _ => DataBaseType.Min
        };
    }
}