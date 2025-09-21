using FCG.Application.UseCases.Example.CreateExample;
using FCG.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace FCG.WebApi.Controllers.v1
{
    [ExcludeFromCodeCoverage]

    public class ExampleController : FcgBaseController
    {
        public ExampleController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CreateExampleOutput>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateExample([FromBody] CreateExampleInput input)
        {
            var output = await _mediator.Send(input, CancellationToken.None).ConfigureAwait(false);

            return Created(string.Empty, ApiResponse<CreateExampleOutput>.SuccesResponse(output));
        }
    }
}
