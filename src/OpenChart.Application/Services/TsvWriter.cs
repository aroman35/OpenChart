using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenChart.Domain.Entities.Candles;

namespace OpenChart.Application.Services
{
    public class TsvWriter : IDisposable, IAsyncDisposable
    {
        private readonly MemoryStream _memoryStream;
        private readonly LineWriter _lineWriter;

        public TsvWriter()
        {
            _memoryStream = new MemoryStream();
            _lineWriter = new LineWriter(_memoryStream);
        }

        public async Task<string> WriteCollectionAsync(IAsyncEnumerable<Candle> candlesCollection, CancellationToken cancellationToken = default)
        {
            await _lineWriter.AddFirstLine(cancellationToken);
            await candlesCollection
                .ForEachAwaitWithCancellationAsync((candle, token) => _lineWriter.AddLineAsync(candle, token), cancellationToken);

            return File();
        }

        private string File()
        {
            var bytes = _memoryStream.ToArray();
            var stringResult = Encoding.UTF8.GetString(bytes);
            return stringResult;
        }

        public void Dispose()
        {
            _memoryStream?.Dispose();
            _lineWriter?.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            try
            {
                Dispose();
                return default;
            }
            catch (Exception exc)
            {
                return new ValueTask(Task.FromException(exc));
            }
        }

        private class LineWriter : IDisposable, IAsyncDisposable
        {
            private readonly MemoryStream _memoryStream;

            public LineWriter(MemoryStream memoryStream)
            {
                _memoryStream = memoryStream;
            }

            public async ValueTask AddFirstLine(CancellationToken cancellationToken)
            {
                await _memoryStream.WriteAsync(Encoding.UTF8.GetBytes("date"), cancellationToken);
                await WriteTab(cancellationToken);
                await _memoryStream.WriteAsync(Encoding.UTF8.GetBytes("open"), cancellationToken);
                await WriteTab(cancellationToken);
                await _memoryStream.WriteAsync(Encoding.UTF8.GetBytes("high"), cancellationToken);
                await WriteTab(cancellationToken);
                await _memoryStream.WriteAsync(Encoding.UTF8.GetBytes("low"), cancellationToken);
                await WriteTab(cancellationToken);
                await _memoryStream.WriteAsync(Encoding.UTF8.GetBytes("close"), cancellationToken);
                await WriteTab(cancellationToken);
                await _memoryStream.WriteAsync(Encoding.UTF8.GetBytes("volume"), cancellationToken);
            }

            private ValueTask WriteTab(CancellationToken cancellationToken)
            {
                return _memoryStream.WriteAsync(Encoding.UTF8.GetBytes("\t"), cancellationToken);
            }

            private ValueTask WriteNewLine(CancellationToken cancellationToken)
            {
                return _memoryStream.WriteAsync(Encoding.UTF8.GetBytes("\n"), cancellationToken);
                // return _memoryStream.WriteAsync(Encoding.UTF8.GetBytes(Environment.NewLine), cancellationToken);
            }

            public async Task AddLineAsync(Candle candle, CancellationToken cancellationToken)
            {
                var date = DateTimeOffset.FromUnixTimeMilliseconds(candle.Date);

                await WriteNewLine(cancellationToken);
                await _memoryStream.WriteAsync(Encoding.UTF8.GetBytes(date.ToString("s")), cancellationToken);
                await WriteTab(cancellationToken);
                await _memoryStream.WriteAsync(Encoding.UTF8.GetBytes(candle.Open.ToString("F8")), cancellationToken);
                await WriteTab(cancellationToken);
                await _memoryStream.WriteAsync(Encoding.UTF8.GetBytes(candle.High.ToString("F8")), cancellationToken);
                await WriteTab(cancellationToken);
                await _memoryStream.WriteAsync(Encoding.UTF8.GetBytes(candle.Low.ToString("F8")), cancellationToken);
                await WriteTab(cancellationToken);
                await _memoryStream.WriteAsync(Encoding.UTF8.GetBytes(candle.Close.ToString("F8")), cancellationToken);
                await WriteTab(cancellationToken);
                await _memoryStream.WriteAsync(Encoding.UTF8.GetBytes(candle.Volume.ToString("F16")), cancellationToken);
            }

            public void Dispose()
            {
                _memoryStream?.Dispose();
            }

            public ValueTask DisposeAsync()
            {
                try
                {
                    Dispose();
                    return default;
                }
                catch (Exception exc)
                {
                    return new ValueTask(Task.FromException(exc));
                }
            }
        }
    }
}