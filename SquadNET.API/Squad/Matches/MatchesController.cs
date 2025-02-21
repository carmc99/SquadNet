using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using SquadNET.Application.Squad.Admin.Commands;
using SquadNET.Application.Squad.Map.Queries;
using SquadNET.Core.Squad.Entities;

/// <summary>
/// Controller for match-related operations in the Squad server.
/// </summary>
[ApiController]
[Route("matches")]
public class MatchesController : ControllerBase
{
    private readonly IMediator Mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchesController"/> class.
    /// </summary>
    /// <param name="mediator">Mediator for handling commands.</param>
    public MatchesController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Ends the current match immediately.
    /// </summary>
    [HttpPost("current/end")]
    public async Task<IActionResult> EndMatch(CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(new EndMatchCommand.Request(), cancellationToken);
        return Ok(new { message = "Match ended", result });
    }

    /// <summary>
    /// Restarts the current match.
    /// </summary>
    [HttpPost("current/restart")]
    public async Task<IActionResult> RestartMatch(CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(new RestartMatchCommand.Request(), cancellationToken);
        return Ok(new { message = "Match restarted", result });
    }

    /// <summary>
    /// Changes the current layer to the specified one.
    /// </summary>
    [HttpPost("layers/change")]
    public async Task<IActionResult> ChangeLayer([FromBody] ChangeLayerCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Layer changed", result });
    }

    /// <summary>
    /// Sets the next layer in the match rotation.
    /// </summary>
    [HttpPost("layers/set-next")]
    public async Task<IActionResult> SetNextLayer([FromBody] SetNextLayerCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Next layer set", result });
    }

    /// <summary>
    /// Retrieves information about the current and next map in the rotation.
    /// </summary>
    [HttpGet("map-info")]
    public async Task<IActionResult> MapInfo(CancellationToken cancellationToken)
    {
        MapInfo result = await Mediator.Send(new MapInfoQuery.Request(), cancellationToken);
        return Ok(result);
    }
}
