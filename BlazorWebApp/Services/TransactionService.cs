using BankSystem.Domain.Entities;
using System.Net.Http.Json;
using BlazorWebApp.DTOs;

public class TransactionService(HttpClient _httpClient)
{
    private async Task<T> GetResponseAsync<T>(HttpResponseMessage response) =>
        await response.Content.ReadFromJsonAsync<T>();

    public async Task<IEnumerable<Transaction>> GetTransactionsAsync() =>
        await GetResponseAsync<IEnumerable<Transaction>>(await _httpClient.GetAsync("https://localhost:7114/api/Transaction/Get/Transactions"));

    public async Task<Transaction> GetTransactionByIdAsync(int id) =>
        await GetResponseAsync<Transaction>(await _httpClient.GetAsync($"https://localhost:7114/api/Transaction/Get/Transaction/{id}"));

    public async Task<IEnumerable<Transaction>> GetTransactionsByAccountNumberAsync(string accountNumber) =>
        await GetResponseAsync<IEnumerable<Transaction>>(await _httpClient.GetAsync($"https://localhost:7114/api/Transaction/GetByAccountNumber/{accountNumber}"));

    public async Task<Responses> CreateTransactionAsync(Transaction transaction) =>
        await GetResponseAsync<Responses>(await _httpClient.PostAsJsonAsync("https://localhost:7114/api/Transaction/Create/Transaction", transaction));

    public async Task<Responses> DeleteTransactionAsync(int transactionId) =>
        await GetResponseAsync<Responses>(await _httpClient.DeleteAsync($"https://localhost:7114/api/Transaction/Delete/Transaction/{transactionId}"));

    public async Task<Responses> DepositAsync(Transaction transaction) =>
        await GetResponseAsync<Responses>(await _httpClient.PostAsJsonAsync("https://localhost:7114/api/Transaction/Deposit/Transaction", transaction));

    public async Task<Responses> WithdrawAsync(Transaction transaction) =>
        await GetResponseAsync<Responses>(await _httpClient.PostAsJsonAsync("https://localhost:7114/api/Transaction/Withdraw/Transaction", transaction));

    public async Task<Responses> TransferAsync(Transaction transaction) =>
        await GetResponseAsync<Responses>(await _httpClient.PostAsJsonAsync("https://localhost:7114/api/Transaction/Transfer/Transaction", transaction));
}
