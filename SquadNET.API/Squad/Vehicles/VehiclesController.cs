using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using SquadNET.Application.Squad.Admin.Commands;

/// <summary>
/// Controller for vehicle-related operations in the Squad server.
/// </summary>
[ApiController]
[Route("vehicles")]
public class VehiclesController : ControllerBase
{
    private readonly IMediator Mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="VehiclesController"/> class.
    /// </summary>
    /// <param name="mediator">Mediator for handling commands.</param>
    public VehiclesController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Disables or enables the vehicle team requirement.
    /// </summary>
    [HttpPost("team-requirement")]
    public async Task<IActionResult> DisableVehicleTeamRequirement([FromBody] DisableVehicleTeamRequirementCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Team requirement disabled", result });
    }

    /// <summary>
    /// Disables or enables vehicle claiming.
    /// </summary>
    [HttpPost("claiming")]
    public async Task<IActionResult> DisableVehicleClaiming([FromBody] DisableVehicleClaimingCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Vehicle claiming disabled", result });
    }
}
