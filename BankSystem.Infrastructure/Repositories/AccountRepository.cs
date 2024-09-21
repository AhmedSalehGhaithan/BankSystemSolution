// The #region directive is typically used to organize large blocks of code for better readability
// this code is not large just i use to demonstrate how it works.
#region Namespaces
using BankSystem.Application.Constant;
using BankSystem.Application.DTOs.AccountDTOs.Request.Account;
using BankSystem.Application.DTOs.AccountDTOs.Response;
using BankSystem.Application.DTOs.AccountDTOs.Response.Account;
using BankSystem.Application.Interface;
using BankSystem.Domain.Authentication;
using BankSystem.Infrastructure.Data;
using BankSystem.SharedLibrarySolution.Logs;
using BankSystem.SharedLibrarySolution.Responses;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
#endregion
namespace BankSystem.Infrastructure.Repositories
{
    public class AccountRepository(
         RoleManager<IdentityRole> _roleManager,
         UserManager<ApplicationUser> _userManager,
         SignInManager<ApplicationUser> _signInManager,
         IConfiguration _config, BankAccountDbContext _context, IAdminConstants _adminConstants) : IAccount
    {
        #region Main Regin

        #region With Main CRUD Operatins

        public async Task<LoginResponse> LoginAccountAsync(LoginDTO model)
        {
            try
            {
                var user = await FindUserByEmailAsync(model.EmailAddress);

                if (user is null) return new LoginResponse(false, "User is not found");

                Microsoft.AspNetCore.Identity.SignInResult result;
                try
                {
                    result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                }
                catch { return new LoginResponse(false, "Invalid credentials"); }

                if (!result.Succeeded) return new LoginResponse(false, "Invalid credentials");

                string JwtToken = await GenerateToken(user);

                string refreshToken = GenerateRefreshToken();

                if (string.IsNullOrEmpty(JwtToken) || string.IsNullOrEmpty(refreshToken))

                    return new LoginResponse(false, "Error occur while logging in  account, Please contact administration");
                else
                {
                    var saveResult = await SaveRefreshToken(user.Id, refreshToken);
                    if (saveResult.Flag)
                        return new LoginResponse(true, $"{user.Name} successfully logged in", JwtToken, refreshToken);
                    else
                        return new LoginResponse();
                }

            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new LoginResponse(false, "Error occurred while deleting");
            }
        }
        public async Task<Response> CreateAccountAsync(CreateAccountDTO model)
        {
            try
            {
                if (await FindUserByEmailAsync(model.EmailAddress) != null)
                    return new Response(false, "Sorry, user is already exist");

                var user = new ApplicationUser()
                {
                    Name = model.Name,
                    UserName = model.EmailAddress,
                    Email = model.EmailAddress,
                    PasswordHash = model.Password
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                string error = CheckResponse(result);
                if (!string.IsNullOrEmpty(error)) return new Response(false, error);

                var (flag, message) = await AssignUserToRole(user, new IdentityRole() { Name = model.Role });
                return new Response(flag, message);

            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occurred while creating account");
            }
        }
        public async Task<Response> DeleteUserAsync(string email)
        {
            try
            {
                var user = await FindUserByEmailAsync(email);
                if (user == null)
                    return new Response(false, "User not found");

                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }

                var result = await _userManager.DeleteAsync(user);

                string error = CheckResponse(result);
                if (!string.IsNullOrEmpty(error)) return new Response(false, error);

                return new Response(true, "User deleted successfully");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occurred while deleting");
            }
        }
        private async Task<ApplicationUser> FindUserByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);
        #endregion

        #region With Roles
        private async Task<IdentityRole> FindRoleByNameAsync(string roleName) => await _roleManager.FindByNameAsync(roleName);
        private async Task<Response> AssignUserToRole(ApplicationUser user, IdentityRole role)
        {
            try
            {
                if (user is null || role is null) return new Response(false, "Model state can not br empty");


                if (await FindRoleByNameAsync(role.Name!) == null) await CreateRoleAsync(role.Adapt(new CreateRoleDTO()));

                LogException.LogToFile($"assign User to role {DateTime.Now} with user Id {user.Id} ");

                IdentityResult result = await _userManager.AddToRoleAsync(user, role.Name!);

                string error = CheckResponse(result);

                if (!string.IsNullOrEmpty(error)) return new Response(false, error);

                else return new Response(true, $"{user.Name} assigned to {role.Name} role");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occurred while assign user to role");
            }
        }
        public async Task<Response> ChangeUserRoleAsync(ChangeUserRoleRequestDTO model)
        {

            try
            {
                if (await FindRoleByNameAsync(model.RoleName) is null) return new Response(false, "Role not found");

                if (await FindUserByEmailAsync(model.userEmail) is null) return new Response(false, "User not found");

                var user = await FindUserByEmailAsync(model.userEmail);

                var previousRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                var removeOldRole = await _userManager.RemoveFromRoleAsync(user, previousRole!);

                var errors = CheckResponse(removeOldRole);

                if (!string.IsNullOrEmpty(errors)) return new Response(false, errors);

                LogException.LogToFile($"change User to role {DateTime.Now} with user Id {user.Id} ");

                var result = await _userManager.AddToRoleAsync(user, model.RoleName);

                var response = CheckResponse(result);

                if (!string.IsNullOrEmpty(response)) return new Response(false, response);

                else return new Response(true, "Role changed");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occurred while changing user role");
            }
        }
        public async Task<Response> CreateRoleAsync(CreateRoleDTO model)
        {
            try
            {
                if ((await FindRoleByNameAsync(model.Name!)) == null)
                {
                    var response = await _roleManager.CreateAsync(new IdentityRole(model.Name!));

                    var error = CheckResponse(response);

                    if (!string.IsNullOrEmpty(error)) throw new Exception(error);

                    else return new Response(true, $"{model.Name} Created");
                }
                LogException.LogToFile($"create role at {DateTime.Now}");

                return new Response(false, $"{model.Name} already created");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occurred while create Role");
            }
        }

        public async Task<IEnumerable<GetRoleDTO>> GetRolesAsync() =>
          (await _roleManager.Roles.ToListAsync()).Adapt<IEnumerable<GetRoleDTO>>();

        public async Task<IEnumerable<GetUsersWithRolesResponseDTO>> GetUsersWithRoles()
        {
            try
            {
                var allUsers = await _userManager.Users.ToListAsync();
                if (allUsers is null) return null!;

                var list = new List<GetUsersWithRolesResponseDTO>();
                foreach (var user in allUsers)
                {
                    var getUserRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                    var getRoleInfo = await _roleManager.Roles.FirstOrDefaultAsync(_r => _r.Name!.ToUpper() == getUserRole!.ToUpper());

                    list.Add(new GetUsersWithRolesResponseDTO()
                    {
                        Name = user.Name,
                        Email = user.Email,
                        RoleId = getRoleInfo!.Id,
                        RoleName = getRoleInfo.Name
                    });
                }

                return list;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                await Console.Out.WriteLineAsync("Error occurred while getting user role");

                return null!;
            }

        }
        #endregion

        #region With Admin
        public async Task CreateAdmin()
        {
            try
            {
                if ((await FindRoleByNameAsync(ConstantValues.Role.Admin)) != null) return;

                var MainAdmins = new CreateAccountDTO()
                {
                    Name = "Admin",
                    EmailAddress = "Admin@gmail.com",
                    Password = "Admin@123",
                    Role = ConstantValues.Role.Admin
                };

                var admin = CreateFirstAdmin();

                await CreateRoleAsync(new CreateRoleDTO() { Name = "Admin" });

                await CreateAccountAsync(admin);

                await CreateAccountAsync(MainAdmins);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                Console.WriteLine($"Error occurred while creating Admin");
            }
        }
        public CreateAccountDTO CreateFirstAdmin()
        {
            var admin = _adminConstants.PrepearAdmin();
            return admin;
        }
        #endregion

        #region With Token

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var userClaims = new[]
                {

                    new Claim(ClaimTypes.Name, user.Email ?? string.Empty),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(ClaimTypes.Role, (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty),
                    new Claim("FullName", user.Name ?? string.Empty) };

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: userClaims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                Console.WriteLine($"Error generating token: {ex.Message}");

                return null!; 
            }
            
        }
        private string CheckResponse(IdentityResult result)
        {
            try
            {
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(_ => _.Description);
                    return string.Join(Environment.NewLine, errors);
                }
                return null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                Console.WriteLine($"{ex.Message}");

                return null!;
            }

        }
        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model)
        {
            try
            {
                var token = await _context.RefreshTokens.FirstOrDefaultAsync(_t => _t.Token == model.Token);
                if (token == null) return new LoginResponse();

                var user = await _userManager.FindByIdAsync(token.UserId!);

                string newRefreshToken = GenerateRefreshToken();


                var saveResult = await SaveRefreshToken(user.Id, newRefreshToken);
                string newToken = await GenerateToken(user!);

                if (saveResult.Flag)
                    return new LoginResponse(true, $"{user.Name} Successfully re-logged in", newToken, newRefreshToken);
                else
                    return new LoginResponse();
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new LoginResponse(false, "Error occurred while refresh token");
            }
            
        }
        private async Task<GeneralResponse> SaveRefreshToken(string userId, string token)
        {
            try
            {
                var user = await _context.RefreshTokens.FirstOrDefaultAsync(_t_ => _t_.UserId == userId);
                if (user is null) _context.RefreshTokens.Add(new RefreshToken() { UserId = userId, Token = token });
                else user.Token = token;

                await _context.SaveChangesAsync();
                return new GeneralResponse(true, null!);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new GeneralResponse(false, "Error occurred while refresh token");
            }
        }
        #endregion

        #endregion
    }
}
