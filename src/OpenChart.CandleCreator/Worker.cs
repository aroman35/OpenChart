using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenChart.Application.Common;

namespace OpenChart.CandleCreator
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IDbContext _dbContext;
        private readonly Stopwatch _stopwatch;

        public Worker(ILogger<Worker> logger, IDbContext dbContext)
        {
            _dbContext = dbContext;
            _stopwatch = Stopwatch.StartNew();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            await _dbContext.SetIndex("SPBXM", "AAPL", DataBaseType.Min);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}