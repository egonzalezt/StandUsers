namespace StandUsers.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.User.Dtos;
using Responses;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ICreateUserUseCase _createUserUseCase;
    private readonly IValidateUserUseCase _validateUserUseCase;

    public UserController(ICreateUserUseCase createUserUseCase, IValidateUserUseCase validateUserUseCase)
    {
        _createUserUseCase = createUserUseCase;
        _validateUserUseCase = validateUserUseCase;
    }

    /// <summary>
    /// Create new user
    /// </summary>
    /// <param name="request">The user data.</param>
    /// <returns>User Id</returns>
    /// <response code="200">User Created</response>
    /// <response code="409">Conflict Email or IdentificationNumber already exists on the system</response>
    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserCreatedResponse>> CreateUserAsync([FromBody]UserDto request)
    {
        var id = await _createUserUseCase.ExecuteAsync(request);
        return Ok(UserCreatedResponse.Build(id));
    }

    [HttpGet("check-email")]
    public async Task<ActionResult<ResourceExistsResponse>> CheckEmailAsync([FromQuery] EmailDto email)
    {
        var emailExists = await _validateUserUseCase.EmailExistsAsync(email.Value);
        var message = emailExists ? "Email already in use" : "Email not in use";
        var response = ResourceExistsResponse.Build(emailExists, message);
        return Ok(response);
    }

    [HttpGet("check-identification")]
    public async Task<ActionResult<ResourceExistsResponse>> CheckIdentificationAsync([FromQuery] int identificationNumber)
    {
        var identificationExists = await _validateUserUseCase.IdentificationNumberExistsAsync(identificationNumber);
        var message = identificationExists ? "Identification already in use" : "Identification not in use";
        var response = ResourceExistsResponse.Build(identificationExists, message);
        return Ok(response);
    }
}
