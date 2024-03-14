namespace StandUsers.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.User.Dtos;
using Responses;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IValidateUserUseCase _validateUserUseCase;

    public UserController(IValidateUserUseCase validateUserUseCase)
    {
        _validateUserUseCase = validateUserUseCase;
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
    public async Task<ActionResult<ResourceExistsResponse>> CheckIdentificationAsync([FromQuery] long identificationNumber)
    {
        var identificationExists = await _validateUserUseCase.IdentificationNumberExistsAsync(identificationNumber);
        var message = identificationExists ? "Identification already in use" : "Identification not in use";
        var response = ResourceExistsResponse.Build(identificationExists, message);
        return Ok(response);
    }
}
