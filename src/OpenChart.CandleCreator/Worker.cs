using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using OpenChart.Application.Common;
using OpenChart.Domain.Entities.Candles;

namespace OpenChart.CandleCreator
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IDbContext _dbContext;

        public Worker(ILogger<Worker> logger, IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var startDate = DateTimeOffset.Parse("03/20/2020 00:00:00+00").ToUnixTimeMilliseconds();
            var endDate = DateTimeOffset.Parse("04/20/2020 00:00:00+00").ToUnixTimeMilliseconds();
            // await _dbContext.SetIndex("SPBXM", "AAPL", DataBaseType.Min);
            var candles = await _dbContext.GetCollection("SPBXM", "AAPL", DataBaseType.Min).AsQueryable()
                .ToListAsync(cancellationToken: stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}