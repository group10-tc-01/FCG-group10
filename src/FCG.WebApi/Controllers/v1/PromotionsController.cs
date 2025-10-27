using FCG.Application.UseCases.Promotions.Create;
using FCG.WebApi.Attributes;
using FCG.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FCG.WebApi.Controllers.v1
{
    public class PromotionsController : FcgBaseController
    {
        public PromotionsController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        [AuthenticatedAdmin]
        [ProducesResponseType(typeof(ApiResponse<CreatePromotionResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreatePromotionRequest input)
        {
            var output = await _mediator.Send(input, CancellationToken.None).ConfigureAwait(false);
            return Created(string.Empty, ApiResponse<CreatePromotionResponse>.SuccesResponse(output));
        }
    }
}
