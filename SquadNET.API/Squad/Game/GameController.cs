using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using SquadNET.Application.Squad.Admin.Commands;

/// <summary>
/// Controller for game-related operations in the Squad server.
/// </summary>
[ApiController]
[Route("game")]
public class GameController : ControllerBase
{
    private readonly IMediator Mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameController"/> class.
    /// </summary>
    /// <param name="mediator">Mediator for handling commands.</param>
    public GameController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Enables or disables fog of war in the game.
    /// </summary>
    [HttpPost("fog-of-war")]
    public async Task<IActionResult> SetFogOfWar([FromBody] SetFogOfWarCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Fog of war updated", result });
    }

    /// <summary>
    /// Sets the maximum number of players allowed in the game.
    /// </summary>
    [HttpPost("max-players")]
    public async Task<IActionResult> SetMaxNumPlayers([FromBody] SetMaxNumPlayersCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Max players updated", result });
    }

    /// <summary>
    /// Adjusts the slomo speed of the game.
    /// </summary>
    [HttpPost("slomo")]
    public async Task<IActionResult> SetSlomoSpeed([FromBody] SlomoCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Slomo speed updated", result });
    }

    /// <summary>
    /// Updates the public queue limit.
    /// </summary>
    [HttpPost("public-queue-limit")]
    public async Task<IActionResult> SetPublicQueueLimit([FromBody] SetPublicQueueLimitCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Public queue limit updated", result });
    }

    /// <summary>
    /// Sets the number of reserved slots in the game.
    /// </summary>
    [HttpPost("reserved-slots")]
    public async Task<IActionResult> SetNumReservedSlots([FromBody] SetNumReservedSlotsCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Reserved slots updated", result });
    }
}
