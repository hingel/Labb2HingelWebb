namespace StoreDataAccess.Repositories;

public interface IOrderRepository<T>
{	
	Task<string> AddItemAsync(T item);
	Task<IEnumerable<T>> GetByEmail(string email);
	Task<bool> DeleteItemAsync(string id);
}
