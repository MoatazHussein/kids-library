using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Salhia.KidsLibrary.Application.Features.AIStories.Commands.GenerateAIStory;
using Salhia.KidsLibrary.Application.Features.AIStories.Commands.RetryAIStorySlide;
using Salhia.KidsLibrary.Application.Features.AIStories.Queries.GenerateAIStoryPdf;
using Salhia.KidsLibrary.Application.Features.AIStories.Queries.GetAIStoryById;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/[Controller]")]
[Authorize]
public class AIStoriesController(
    IMediator mediator,
    ILogger<AIStoriesController> logger) : ControllerBase
{
    [HttpPost("GetById")]
    public async Task<IActionResult> GetById(GetAIStoryByIdQuery query)
    {
        var aiStory = await mediator.Send(query);
        return Ok(aiStory);
    }

    [HttpPost("Generate")]
    [EnableRateLimiting("AIStoryGenerationPolicy")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> Generate(GenerateAIStoryCommand command)
    {
        var response = await mediator.Send(command);
        
        return Ok(response);
    }

    [HttpPost("Slides/{slideId}/Retry")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RetrySlide([FromRoute] string slideId)
    {
        var response = await mediator.Send(new RetryAIStorySlideCommand { SlideId = slideId });
        
        return Ok(response);
    }

    [HttpGet("DownloadPdf/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadPdf([FromRoute] string id)
    {
        var pdfBytes = await mediator.Send(new GenerateAIStoryPdfQuery(id));

        return File(pdfBytes, "application/pdf", $"ai-story-{id}.pdf");
    }

}
