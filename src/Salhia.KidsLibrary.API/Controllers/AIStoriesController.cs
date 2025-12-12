using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Features.AIStories.Commands.GenerateAIStory;
using Salhia.KidsLibrary.Application.Features.AIStories.Queries.GenerateAIStoryPdf;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/[Controller]")]
[Authorize]
public class AIStoriesController(IMediator mediator, ILogger<AIStoriesController> logger) : ControllerBase
{

    /// <summary>
    /// Generates an AI story with slides using OpenAI
    /// </summary>
    /// <param name="command">Story generation parameters</param>
    /// <returns>AIStory ID</returns>
    [HttpPost("Generate")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Generate(GenerateAIStoryCommand command)
    {
        logger.LogInformation(
            "Received request to generate AI story. CustomStoryId={CustomStoryId}, StoryName={StoryName}",
            command.CustomStoryId, command.StoryName);

        string aiStoryId = await mediator.Send(command);
        
        logger.LogInformation("AI story generated successfully. AIStoryId={AIStoryId}", aiStoryId);
        
        return StatusCode(201, new 
        { 
            message = "AI story generation started successfully",
            aiStoryId = aiStoryId,
            status = "Slides are being generated in the background"
        });
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
