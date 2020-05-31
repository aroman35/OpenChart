using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenChart.Application.Common;
using OpenChart.Application.Services;
using OpenChart.Domain.Entities;
using OpenChart.Domain.Entities.Candles;
using OpenChart.Domain.Extensions;

namespace OpenChart.CandleCreator
{
    public class Worker : BackgroundService
    {
        private readonly IDbContext _dbContext;
        private readonly ILogger<Worker> _logger;

        public Worker(IDbContext dbContext, ILogger<Worker> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var exchange = new Exchange
            {
                ClassCode = "SPMXM",
                TradeStart = (long) TimeSpan.FromHours(7).TotalMilliseconds
            };

            var candlesService = new CandlesService();
            candlesService.CurrentInstrument(exchange, "AAPL");

            using var source = File.OpenText("AAPL@SPBXM_M1.json");
            var json = await source.ReadToEndAsync();

            var m1candles = JsonConvert.DeserializeObject<List<Candle>>(json).ToLinkedList();
            var h1Candles = candlesService.ChangeTimeFrame(m1candles, TimeFrame.H1).ToLinkedList();
            var d1Candles = candlesService.ChangeTimeFrame(m1candles, TimeFrame.D).ToLinkedList();

            await _dbContext.GetCollection("SPBXM", "AAPL", DataBaseType.Min)
                .InsertManyAsync(m1candles, cancellationToken: stoppingToken);
            await _dbContext.GetCollection("SPBXM", "AAPL", DataBaseType.Hour)
                .InsertManyAsync(h1Candles, cancellationToken: stoppingToken);
            await _dbContext.GetCollection("SPBXM", "AAPL", DataBaseType.Day)
                .InsertManyAsync(d1Candles, cancellationToken: stoppingToken);

            await _dbContext.SetIndex("SPBXM", "AAPL", DataBaseType.Min);
            await _dbContext.SetIndex("SPBXM", "AAPL", DataBaseType.Hour);
            await _dbContext.SetIndex("SPBXM", "AAPL", DataBaseType.Day);

            _logger.LogInformation("Test data added.");
        }
    }
}