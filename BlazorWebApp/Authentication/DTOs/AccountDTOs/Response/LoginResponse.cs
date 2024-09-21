namespace BlazorWebApp.Authentication.DTOs.Accounts.Response
{
    public record LoginResponse(bool Flag = false, string Message = null!, string Token = null!, string RefreshToken = null!);

}
