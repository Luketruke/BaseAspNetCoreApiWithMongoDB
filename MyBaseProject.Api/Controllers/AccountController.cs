using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBaseProject.Application.DTOs.Requests;
using MyBaseProject.Application.DTOs.Responses;
using MyBaseProject.Application.Interfaces.Services;

namespace MyBaseProject.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of all user accounts.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountResponseDto>>> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        /// <summary>
        /// Create a new account (requires `Password`).
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<AccountResponseDto>> CreateAccount([FromBody] AccountCreateRequestDto request)
        {
            var createdAccount = await _accountService.CreateAccountAsync(request);
            return CreatedAtAction(nameof(GetAccountById), new { id = createdAccount.AccountId }, createdAccount);
        }

        /// <summary>
        /// Gets an account through its ObjectId.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountResponseDto>> GetAccountById(string id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            return Ok(account);
        }

        /// <summary>
        /// Updates an account by ID (may optionally include `Password`).
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(string id, [FromBody] AccountUpdateRequestDto request)
        {
            await _accountService.UpdateAccountAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// An account is deleted based on an ObjectId.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            await _accountService.DeleteAccountAsync(id);
            return NoContent();
        }
    }
}
