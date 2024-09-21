using BankSystem.SharedLibrarySolution.Responses;
namespace BankSystem.SharedLibrarySolution.Interface
{
    public interface IGenericInterface<T> where T : class
    {
        Task<Response> CreateAsync(T entity);
        Task<Response> DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> FindByIdAsync(int id);
    }
}
