using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using SquadNET.Application.Squad.Admin.Commands;
using SquadNET.Application.Squad.Player.Queries;
using SquadNET.Core.Squad.Models;

/// <summary>
/// Controller for player-related operations in the Squad server.
/// </summary>
[ApiController]
[Route("players")]
public class PlayersController : ControllerBase
{
    private readonly IMediator Mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayersController"/> class.
    /// </summary>
    /// <param name="mediator">Mediator for handling commands.</param>
    public PlayersController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Demotes a commander by player ID.
    /// </summary>
    [HttpPost("{playerId}/demote")]
    public async Task<IActionResult> DemoteCommanderById([FromRoute] int playerId, CancellationToken cancellationToken)
    {
        DemoteCommanderByIdCommand.Request request = new() { PlayerId = playerId };
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Player demoted", result });
    }

    /// <summary>
    /// Demotes a commander by player name.
    /// </summary>
    [HttpPost("demote")]
    public async Task<IActionResult> DemoteCommander([FromBody] DemoteCommanderCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Player demoted", result });
    }

    /// <summary>
    /// Bans a player by name.
    /// </summary>
    [HttpPost("ban")]
    public async Task<IActionResult> BanPlayer([FromBody] BanPlayerCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Player banned", result });
    }

    /// <summary>
    /// Bans a player by ID.
    /// </summary>
    [HttpPost("{playerId}/ban")]
    public async Task<IActionResult> BanPlayerById([FromRoute] int playerId, [FromBody] BanPlayerByIdCommand.Request request, CancellationToken cancellationToken)
    {
        request.PlayerId = playerId;
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Player banned", result });
    }

    /// <summary>
    /// Kicks a player by name.
    /// </summary>
    [HttpPost("kick")]
    public async Task<IActionResult> KickPlayer([FromBody] KickPlayerCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Player kicked", result });
    }

    /// <summary>
    /// Kicks a player by ID.
    /// </summary>
    [HttpPost("{playerId}/kick")]
    public async Task<IActionResult> KickPlayerById([FromRoute] int playerId, [FromBody] KickPlayerByIdCommand.Request request, CancellationToken cancellationToken)
    {
        request.PlayerId = playerId;
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Player kicked", result });
    }

    /// <summary>
    /// Issues a warning to a player by name.
    /// </summary>
    [HttpPost("warn")]
    public async Task<IActionResult> WarnPlayer([FromBody] WarnPlayerCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Player warned", result });
    }

    /// <summary>
    /// Issues a warning to a player by ID.
    /// </summary>
    [HttpPost("{playerId}/warn")]
    public async Task<IActionResult> WarnPlayerById([FromRoute] int playerId, [FromBody] WarnPlayerByIdCommand.Request request, CancellationToken cancellationToken)
    {
        request.PlayerId = playerId;
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Player warned", result });
    }

    /// <summary>
    /// Restricts team changes for a specified duration.
    /// </summary>
    [HttpPost("no-team-change-timer")]
    public async Task<IActionResult> NoTeamChangeTimer([FromBody] NoTeamChangeTimerCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Team change restricted", result });
    }

    /// <summary>
    /// Forces a player to change to a specific team.
    /// </summary>
    [HttpPost("{playerId}/force-team-change")]
    public async Task<IActionResult> ForceTeamChange([FromRoute] int playerId, [FromBody] ForceTeamChangeByIdCommand.Request request, CancellationToken cancellationToken)
    {
        request.PlayerId = playerId;
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Player moved to team", result });
    }

    /// <summary>
    /// Forces a player to change to a specific team.
    /// </summary>
    [HttpPost("force-team-change")]
    public async Task<IActionResult> ForceTeamChange([FromBody] ForceTeamChangeCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Player moved to team", result });
    }

    /// <summary>
    /// Retrieves the list of all players currently on the server.
    /// </summary>
    [HttpGet("list-players")]
    public async Task<IActionResult> ListPlayers(CancellationToken cancellationToken)
    {
        ListPlayerModel result = await Mediator.Send(new ListPlayersQuery.Request(), cancellationToken);
        return Ok(result);
    }
}
