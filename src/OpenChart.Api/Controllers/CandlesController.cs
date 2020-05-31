using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenChart.Application.Queries;
using OpenChart.Domain.Entities;

namespace OpenChart.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandlesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CandlesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTest(
            string classCode,
            string securityCode,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            TimeFrame timeFrame,
            CancellationToken cancellationToken)
        {
            var candlesResponse = await _mediator.Send(new GetTsvCandlesQuery(classCode, securityCode, startDate, endDate, timeFrame),
                cancellationToken);
            Response.ContentType = "text/tab-separated-values;charset=utf-8";
            return Content(candlesResponse);
        }
    }
}