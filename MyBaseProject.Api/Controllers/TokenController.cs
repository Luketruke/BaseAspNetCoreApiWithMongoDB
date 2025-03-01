using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBaseProject.Application.DTOs.Requests;
using MyBaseProject.Application.DTOs.Requests.Login;
using MyBaseProject.Application.DTOs.Responses;
using MyBaseProject.Application.Interfaces.Services;
using MyBaseProject.Domain.Enums;

[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly IAuthService _loginService;
    private readonly IAccountService _accountService;
    private readonly IJwtService _jwtService;
    
    public TokenController(IAuthService loginService, IAccountService accountService, IJwtService jwtService)
    {
        _loginService = loginService;
        _accountService = accountService;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Generates a JWT token based on the user's credentials.
    /// </summary>
    [HttpPost("generate")]
    public async Task<IActionResult> GenerateToken([FromBody] LoginRequestDto request)
    {
        var account = await _loginService.AuthenticateAsync(request.Email, request.Password);
        if (account == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var token = _jwtService.GenerateToken(account.AccountId, account.Email);

        return Ok(new { token });
    }

    /// <summary>
    /// Authenticates a user using a login provider (Google or Facebook).
    /// </summary>
    [HttpPost("social-login")]
    public async Task<IActionResult> SocialLogin([FromBody] SocialLoginRequestDto request)
    {
        AccountResponseDto? account = request.Provider switch
        {
            LoginProvider.Google => await _loginService.AuthenticateWithGoogleAsync(request.Token),
            LoginProvider.Facebook => await _loginService.AuthenticateWithFacebookAsync(request.Token),
            _ => null
        };

        if (account == null)
        {
            return Unauthorized(new { message = "Invalid social login credentials" });
        }

        var token = _jwtService.GenerateToken(account.AccountId, account.Email);
        return Ok(new { token });
    }

    /// <summary>
    /// Obtains the authenticated user's data from the token.
    /// </summary>
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetUserData()
    {
        if (!HttpContext.Request.Headers.TryGetValue("Authorization", out var tokenValue))
        {
            return Unauthorized(new { message = "Authorization header is missing" });
        }

        var token = tokenValue.ToString().Replace("Bearer ", "").Trim();
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        var userId = _jwtService.GetUserIdFromToken(token);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        var account = await _accountService.GetAccountByIdAsync(userId);
        if (account == null)
        {
            return NotFound(new { message = "User not found" });
        }

        return Ok(account);
    }
}