using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OpenChart.Application.Common;
using OpenChart.Domain.Entities;
using OpenChart.Domain.Entities.Candles;
using OpenChart.Domain.Extensions;

namespace OpenChart.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandlesController : ControllerBase
    {
        private readonly IDbContext _dbContext;
        private readonly ICandleDomainService _candleDomainService;

        public CandlesController(
            IDbContext dbContext,
            ICandleDomainService candleDomainService)
        {
            _dbContext = dbContext;
            _candleDomainService = candleDomainService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Candle>>> GetTest(
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            TimeFrame timeFrame,
            CancellationToken cancellationToken)
        {
            var startDateMs = startDate.ToUnixTimeMilliseconds();
            var endDateMs = endDate.ToUnixTimeMilliseconds();
            var exchange = new Exchange
            {
                ClassCode = "SPBXM",
                TradeStart = (long)TimeSpan.FromHours(7).TotalMilliseconds
            };
            _candleDomainService.CurrentInstrument(exchange, "AAPL");

            var candles = _dbContext.GetCollection("SPBXM", "AAPL", DataBaseType.Min)
                .AsQueryable()
                .Where(x => x.Date > startDateMs && x.Date < endDateMs)
                .ToAsyncEnumerable();

            return await _candleDomainService.ChangeTimeFrame(candles, timeFrame, cancellationToken).ToLinkedListAsync(cancellationToken);
        }
    }
}