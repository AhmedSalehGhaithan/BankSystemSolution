#region Account Controller
using BankSystem.Application.DTOs.AccountDTOs.Request.Account;
using BankSystem.Application.DTOs.AccountDTOs.Response.Account;
using BankSystem.Application.Interface;
using BankSystem.SharedLibrarySolution.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccount _account) : ControllerBase
    {
        [HttpPost("identity/create")]
        public async Task<ActionResult<Response>> CreateAccount(CreateAccountDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model can not be null");
            return Ok(await _account.CreateAccountAsync(model));
        }

        [HttpPost("identity/login")]
        public async Task<ActionResult<Response>> LoginAccount(LoginDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model can not be null");
            return Ok(await _account.LoginAccountAsync(model));
        }

        [HttpDelete("identity/delete/account/{email}")]

        public async Task<ActionResult<Response>> DeleteAccount(string email)
        {
            if (!ModelState.IsValid) return BadRequest("ID can not be null");
            var response = await _account.DeleteUserAsync(email);
            return Ok(response);
        }
        [HttpPost("identity/refresh-token")]
        public async Task<ActionResult<Response>> RefreshToken(RefreshTokenDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model can not be null");
            return Ok(await _account.RefreshTokenAsync(model));
        }

        [HttpPost("identity/create/role")]
        public async Task<ActionResult<Response>> CreateRole(CreateRoleDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model can not be null");
            return Ok(await _account.CreateRoleAsync(model));
        }

        [HttpGet("identity/role/list")]
        public async Task<ActionResult<IEnumerable<GetRoleDTO>>> GetRoles() => Ok(await _account.GetRolesAsync());

        [HttpPost("/setting")]
        public async Task<IActionResult> CreateAdmin()
        {
            await _account.CreateAdmin();
            return Ok();
        }

        [HttpGet("identity/users-with-role")]
        public async Task<ActionResult<IEnumerable<GetUsersWithRolesResponseDTO>>> GetUsersWithRole()
            => Ok(await _account.GetUsersWithRoles());

        [HttpPost("identity/change-role")]
        public async Task<ActionResult<Response>> ChangeUserRole(ChangeUserRoleRequestDTO model)
            => Ok(await _account.ChangeUserRoleAsync(model));
    }
}
#endregion