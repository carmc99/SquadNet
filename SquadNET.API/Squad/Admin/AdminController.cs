using MediatR;
using Microsoft.AspNetCore.Mvc;
using SquadNET.Application.Squad.Admin.Commands;
using System.Threading.Tasks;
using System.Threading;
using SquadNET.Application.Squad.Admin.Queries;
using SquadNET.Application.Squad.Map.Queries;
using SquadNET.Application.Squad.Player.Queries;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Models;

/// <summary>
/// Controller for administrative operations in the Squad server.
/// </summary>
[ApiController]
[Route("admin")]
public class AdminController : ControllerBase
{
    private readonly IMediator Mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminController"/> class.
    /// </summary>
    /// <param name="mediator">Mediator for handling commands.</param>
    public AdminController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Executes a raw RCON command on the server.
    /// </summary>
    [HttpPost("execute-command")]
    public async Task<IActionResult> ExecuteRawCommand([FromBody] ExecuteRawCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Command executed", result });
    }

    /// <summary>
    /// Adds a player as a cameraman in the server.
    /// </summary>
    [HttpPost("add-cameraman")]
    public async Task<IActionResult> AddCameraman([FromBody] AddCameramanCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Cameraman added", result });
    }

    /// <summary>
    /// Sends a broadcast message to all players in the server.
    /// </summary>
    [HttpPost("broadcast-message")]
    public async Task<IActionResult> BroadcastMessage([FromBody] BroadcastMessageCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Message broadcasted", result });
    }

    /// <summary>
    /// Retrieves the list of available admin commands.
    /// </summary>
    [HttpGet("commands")]
    public async Task<IActionResult> ListCommands(CancellationToken cancellationToken)
    {
        List<CommandInfo> result = await Mediator.Send(new ListCommandsQuery.Request(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the list of available layers in the server.
    /// </summary>
    [HttpGet("list-layers")]
    public async Task<IActionResult> ListLayers(CancellationToken cancellationToken)
    {
        List<LayerInfo> result = await Mediator.Send(new ListLayersQuery.Request(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the list of available levels in the server.
    /// </summary>
    [HttpGet("list-levels")]
    public async Task<IActionResult> ListLevels(CancellationToken cancellationToken)
    {
        List<LevelInfo> result = await Mediator.Send(new ListLevelsQuery.Request(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the current and next map in the rotation.
    /// </summary>
    [HttpGet("map-info")]
    public async Task<IActionResult> MapInfo(CancellationToken cancellationToken)
    {
        MapInfo result = await Mediator.Send(new MapInfoQuery.Request(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the list of players currently on the server.
    /// </summary>
    [HttpGet("list-players")]
    public async Task<IActionResult> ListPlayers(CancellationToken cancellationToken)
    {
        ListPlayerModel result = await Mediator.Send(new ListPlayersQuery.Request(), cancellationToken);
        return Ok(result);
    }
}
