using System;
using MediatR;
using OpenChart.Domain.Entities;

namespace OpenChart.Application.Queries
{
    public class GetTsvCandlesQuery : IRequest<string>
    {
        public readonly string ClassCode;
        public readonly string SecurityCode;
        public readonly long StartDate;
        public readonly long EndDate;
        public readonly TimeFrame TimeFrame;

        public GetTsvCandlesQuery(string classCode, string securityCode, DateTimeOffset startDate, DateTimeOffset endDate, TimeFrame timeFrame)
        {
            ClassCode = classCode;
            SecurityCode = securityCode;
            StartDate = startDate.ToUnixTimeMilliseconds();
            EndDate = endDate.ToUnixTimeMilliseconds();
            TimeFrame = timeFrame;
        }
    }
}