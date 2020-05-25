using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using OpenChart.Application.Common;
using OpenChart.Domain.Entities;
using OpenChart.Domain.Services;

namespace OpenChart.Application.Queries.CandlesByCount
{
    public class CandlesCountHandler : IRequestHandler<CandlesCountRequest, IEnumerable<Candle>>
    {
        private readonly TimeSpan _tradeDateStart;
        private readonly IDbContext _dbContext;
        private readonly ICandleDomainService _candleDomainService;

        public CandlesCountHandler(IDbContext dbContext, ICandleDomainService candleDomainService)
        {
            _dbContext = dbContext;
            _candleDomainService = candleDomainService;
            _tradeDateStart = TimeSpan.FromHours(6);
        }

        public async Task<IEnumerable<Candle>> Handle(CandlesCountRequest request, CancellationToken cancellationToken)
        {
            var builder = Builders<CandleDto>.Filter;
            var filter = builder.Lte("date", request.DateLimit);

            var dtoResult = await _dbContext.GetCollection(request.ClassCode, request.SecurityCode)
                .Find(filter)
                // .Limit(request.CandlesCount)
                .ToListAsync(cancellationToken);
            //
            // _candleDomainService.CurrentInstrument(new InstrumentInfo(
            //     new TradeInstrument(request.ClassCode, request.SecurityCode),
            //     _tradeDateStart));

            // var candlesResult = _candleDomainService.ChangeTimeFrame(dtoResult, request.TimeFrame);

            throw new NotImplementedException();
            // return candlesResult;
        }
    }
}