using WebApplication2.Models;

namespace WebApplication2.Repository
{
    public interface IRepo<T> where T : class
    {
        string ReturnString(string x);

        Task<List<T>> CartItemsAsync();

        Task<T> PostItemsAsync(T t); 
    }
}
