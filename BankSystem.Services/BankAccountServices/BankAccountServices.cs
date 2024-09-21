using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BankSystem.Application.DTOs;
using BankSystem.Services.BankAccountServices;
using BankSystem.SharedLibrarySolution.Responses;

public class BankAccountServices
{
    private readonly HttpClient _httpClient;

    public BankAccountServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Fetch all bank accounts
    public async Task<IEnumerable<BankAccountDTOs>> GetAccountsAsync()
    {
        var response = await _httpClient.GetAsync("api/BankAccount/GetAccounts");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<BankAccountDTOs>>();
    }

    // Fetch a single bank account by ID
    public async Task<BankAccountDTO> GetAccountByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/BankAccount/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BankAccountDTO>();
    }

    // Fetch a bank account by account number
    public async Task<BankAccountDTO> GetAccountByNumberAsync(string accountNumber)
    {
        var response = await _httpClient.GetAsync($"api/BankAccount/GetByAccountNumber/{accountNumber}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BankAccountDTO>();
    }

    // Create a new bank account
    public async Task<Response> CreateAccountAsync(BankAccountDTO accountDTO)
    {
        var response = await _httpClient.PostAsJsonAsync("api/BankAccount", accountDTO);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Response>();
    }

    // Update an existing bank account
    public async Task<Response> UpdateAccountAsync(BankAccountDTO accountDTO)
    {
        var response = await _httpClient.PutAsJsonAsync("api/BankAccount", accountDTO);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Response>();
    }

    // Delete a bank account
    public async Task<Response> DeleteAccountAsync(BankAccountDTO accountDTO)
    {
        var response = await _httpClient.DeleteAsync($"api/BankAccount/{accountDTO.Id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Response>();
    }
}
