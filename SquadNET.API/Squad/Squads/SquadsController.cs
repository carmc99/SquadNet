using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using SquadNET.Application.Squad.Admin.Commands;

/// <summary>
/// Controller for squad-related operations in the Squad server.
/// </summary>
[ApiController]
[Route("squads")]
public class SquadsController : ControllerBase
{
    private readonly IMediator Mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="SquadsController"/> class.
    /// </summary>
    /// <param name="mediator">Mediator for handling commands.</param>
    public SquadsController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Disbands a squad in a specified team.
    /// </summary>
    [HttpPost("{squadId}/disband")]
    public async Task<IActionResult> DisbandSquad([FromRoute] int squadId, [FromBody] DisbandSquadCommand.Request request, CancellationToken cancellationToken)
    {
        request.SquadId = squadId;
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Squad disbanded", result });
    }

    /// <summary>
    /// Removes a player from a squad by name.
    /// </summary>
    [HttpPost("remove-player")]
    public async Task<IActionResult> RemovePlayerFromSquad([FromBody] RemovePlayerFromSquadCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Player removed from squad", result });
    }

    /// <summary>
    /// Removes a player from a squad by ID.
    /// </summary>
    [HttpPost("{squadId}/remove-player")]
    public async Task<IActionResult> RemovePlayerFromSquadById([FromRoute] int playerId, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(new RemovePlayerFromSquadByIdCommand.Request
        {
            PlayerId = playerId,
        }, cancellationToken);
        return Ok(new { message = "Player removed from squad", result });
    }
}
