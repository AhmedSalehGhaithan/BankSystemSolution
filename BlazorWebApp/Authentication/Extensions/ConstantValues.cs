namespace BlazorWebApp.Authentication.DTOs.Extensions { 
    public static class ConstantValues
    {
        private const string General = "api/account/identity";
        public const string RegisterRoute = $"{General}/create";
        public const string LoginRoute = $"{General}/login";
        public const string RefreshTokenRoute = $"{General}/refresh-token";
        public const string DeleteUserAccount = $"{General}/delete/account";
        public const string GetRoleRoute = $"{General}/role/list";
        public const string CreateRoleRoute = $"{General}/create/role";
        public const string CreateAdminRoute = $"setting";
        public const string GetUsersWithRoleRoute = $"{General}/users-with-role";
        public const string ChangeUserRoleRoute = $"{General}/change-role";
        public const string BrowserStorageKey = "x-key";
        public const string HttpClientName = "WebUiClient";
        public const string AuthenticationType = "JwtAuth";
        public const string HttpClientHeaderScheme = "Bearer";
        public static class Role
        {
            public const string Admin = "Admin";
            public const string User = "User";
        }

    }
}
