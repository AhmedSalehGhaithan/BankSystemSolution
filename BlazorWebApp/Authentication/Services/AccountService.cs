using BlazorWebApp.Authentication.DTOs.Accounts.Request.Account;
using BlazorWebApp.Authentication.DTOs.Accounts.Response;
using BlazorWebApp.Authentication.DTOs.Accounts.Response.Account;
using BlazorWebApp.Authentication.DTOs.Extensions;
using BlazorWebApp.DTOs;
using System.Net.Http.Json;
namespace BlazorWebApp.Authentication.Service
{
    public class AccountService(HttpClientService _httpClientService) : IAccountService
    {
        public async Task<Responses> ChangeUserRoleAsync(ChangeUserRoleRequest model)
        {
            try
            {
                var publicClient = await _httpClientService.GetPrivateClient();
                var response = await publicClient.PostAsJsonAsync(ConstantValues.ChangeUserRoleRoute, model);

                string error = CheckResponseState(response);
                if(!string.IsNullOrEmpty(error)) return new Responses(false, error);

                var result = await response.Content.ReadFromJsonAsync<Responses>();
                return result!;

            }catch (Exception ex) { return new Responses(false, ex.Message); }
        }

        public async Task<Responses> RegisterAccountAsync(CreateAccount model)
        {
            try
            {
                var publicClient = _httpClientService.GetPublicClient();
                var response = await publicClient.PostAsJsonAsync(ConstantValues.RegisterRoute, model);

                string error = CheckResponseState(response);
                if (!string.IsNullOrEmpty(error)) return new Responses(Flag: false, Message: error);

                var result = await response.Content.ReadFromJsonAsync<Responses>();
                return result!;
            }
            catch (Exception ex) { return new Responses(Flag: false, Message: ex.Message); }
        }

        public static string CheckResponseState(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return $"Sorry, Unknown error occurred.{Environment.NewLine}Error Description : {Environment.NewLine}Status code : {response.StatusCode}{Environment.NewLine}Reason Phrase : {response.ReasonPhrase}";
            else
                return null!;
        }
        public async Task CreateAdminAtFirstStart()
        {
            try
            {
                var client = _httpClientService.GetPublicClient();
                await client.PostAsync(ConstantValues.CreateAdminRoute, null);

            }catch { }
        }

        public async Task<Responses> CreateRoleAsync(CreateRole model)
        {
            try
            {
                var client = await _httpClientService.GetPrivateClient();
                var response = await client.PostAsJsonAsync(ConstantValues.CreateRoleRoute, model);

                string error = CheckResponseState(response);
                if (!string.IsNullOrEmpty(error)) return new Responses(Flag: false, Message: error);

                var result = await response.Content.ReadFromJsonAsync<Responses>();
                return result!;

            }
            catch(Exception ex) { return new Responses(false, ex.Message); }
        }

        public async Task<IEnumerable<GetRole>> GetRolesAsync()
        {
            try
            {
                var privateClient = await _httpClientService.GetPrivateClient();
                var response = await privateClient.GetAsync(ConstantValues.GetRoleRoute);

                string error = CheckResponseState(response);
                if (!string.IsNullOrEmpty(error)) throw new Exception(error);

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetRole>>();
                return result!;
            }
            catch(Exception ex) { throw new Exception(ex.Message); }
        }

        //not used
        //public IEnumerable<GetRole> GetDefaultRoles()
        //{
        //    var list = new List<GetRole>();
        //    list.Clear();
        //    list.Add(new GetRole(1.ToString(), ConstantValues.Role.Admin));
        //    list.Add(new GetRole(2.ToString(), ConstantValues.Role.User));
        //    return list;
        //}
        public async Task<IEnumerable<GetUsersWithRolesResponse>> GetUsersWithRolesAsync()
        {
            try
            {
                var privateClient = await _httpClientService.GetPrivateClient();
                var response = await privateClient.GetAsync(ConstantValues.GetUsersWithRoleRoute);

                string error = CheckResponseState(response);
                if (!string.IsNullOrEmpty(error)) throw new Exception(error);

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetUsersWithRolesResponse>>();
                return result!;
            }catch(Exception ex) { throw new Exception(ex.Message) ; }
        }

        public async Task<LoginResponse> LoginAccountAsync(LoginModel model)
        {
            try
            {
                var publicClient = _httpClientService.GetPublicClient();
                var response = await publicClient.PostAsJsonAsync(ConstantValues.LoginRoute, model);
                string error = CheckResponseState(response);
                if (!string.IsNullOrEmpty(error))
                    return new LoginResponse(false, error);

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result!;
            }catch (Exception ex) { return new LoginResponse(false, ex.Message); }
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken model)
        {
            try
            {
                var publicClient = _httpClientService.GetPublicClient();
                var response = await publicClient.PostAsJsonAsync(ConstantValues.RefreshTokenRoute, model);

                string error = CheckResponseState(response);
                if (!string.IsNullOrEmpty(error)) return new LoginResponse(false, error);

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result!;
            }
            catch (Exception ex) { return new LoginResponse(false, ex.Message); }
        }

        public async Task<Responses> DeleteAccountAsync(string email)
        {
            try
            {
                var client = await _httpClientService.GetPrivateClient();
                var response = await client.DeleteAsync($"{ConstantValues.DeleteUserAccount}/{email}");

                string error = CheckResponseState(response);
                if (!string.IsNullOrEmpty(error)) return new Responses(false, error);

                var result = await response.Content.ReadFromJsonAsync<Responses>();
                return result!;
            }
            catch (Exception ex) { return new Responses(false, ex.Message); }
        }

    }
}


