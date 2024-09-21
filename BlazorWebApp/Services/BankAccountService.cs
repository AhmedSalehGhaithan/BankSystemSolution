using BankSystem.Domain.Entities;
using System.Net.Http.Json;
using BlazorWebApp.DTOs;

public class BankAccountService(HttpClient _httpClient)
{
    private async Task<T> GetResponseAsync<T>(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<IEnumerable<BankAccount>> GetAccountsAsync() =>
        await GetResponseAsync<IEnumerable<BankAccount>>(await _httpClient.GetAsync("https://localhost:7114/api/BankAccount/GetAccounts"));

    public async Task<BankAccount> GetAccountByIdAsync(int id) =>
        await GetResponseAsync<BankAccount>(await _httpClient.GetAsync($"https://localhost:7114/api/BankAccount/Get/Account/{id}"));

    public async Task<BankAccount> GetAccountByNumberAsync(string accountNumber) =>
        await GetResponseAsync<BankAccount>(await _httpClient.GetAsync($"https://localhost:7114/api/BankAccount/GetByAccountNumber/{accountNumber}"));



    public async Task<Responses> UpdateAccountAsync(BankAccount account) =>
        await GetResponseAsync<Responses>(await _httpClient.PutAsJsonAsync("https://localhost:7114/api/BankAccount/Update/Account", account));

    public async Task<Responses> DeleteAccountAsync(int accountId) =>
        await GetResponseAsync<Responses>(await _httpClient.DeleteAsync($"https://localhost:7114/api/BankAccount/Delete/Account/{accountId}"));

    public async Task<Responses> CreateAccountAsync(BankAccount account)
    {
        if (string.IsNullOrEmpty(account.PhoneNumber) && string.IsNullOrEmpty(account.FullName))
        {
            return new Responses(false, "Model can't be empty");
        }

        if (account.Balance < 1000)
        {
            return new Responses(false, "Can't add bank with less than 1000");
        }

        var response = await _httpClient.PostAsJsonAsync("https://localhost:7114/api/BankAccount/Create/Account", account);
        return await GetResponseAsync<Responses>(response);
    }
}
