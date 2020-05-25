using System.Threading;
using System.Threading.Tasks;
using OpenChart.Domain.Entities;
using OpenChart.Domain.Extensions;
using OpenChart.Domain.Services;
using OpenChart.Tests.Unit.Infrastructure;
using Shouldly;
using Xunit;

namespace OpenChart.Tests.Unit
{
    public class CandleDomainServiceTests
    {
        private const string ClassCode = "SPBXM";
        private const string SecurityCode = "AAPL";
        private readonly CollectionLoader _collectionLoader;
        private readonly Exchange _exchange;
        private readonly ICandleDomainService _candleDomainService;
        private readonly CancellationToken _cancellationToken;

        public CandleDomainServiceTests()
        {
            _collectionLoader = new CollectionLoader();
            _exchange = Fakes.MockExchange();
            _candleDomainService = new CandleDomainService();
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task Test1()
        {
            var testDtoCollection = _collectionLoader.LoadTestDataDto(ClassCode, SecurityCode);
            var testCollection = _collectionLoader.LoadTestData(ClassCode, SecurityCode);
            _candleDomainService.CurrentInstrument(_exchange, SecurityCode);

            var candlesCollection = await _candleDomainService.Create(testDtoCollection, _cancellationToken).ToLinkedListAsync(_cancellationToken);

            var m5Aggregate = await _candleDomainService.ChangeTimeFrame(testCollection, TimeFrame.M5, _cancellationToken)
                .ToLinkedListAsync(_cancellationToken);

            var h4Aggregate = await _candleDomainService.ChangeTimeFrame(testCollection, TimeFrame.H4, _cancellationToken)
                .ToLinkedListAsync(_cancellationToken);

            candlesCollection.ShouldNotBeNull();
        }
    }
}