namespace StoreDataAccess.Repositories;

public interface IOrderRepository<T>
{	
	Task<string> AddItemAsync(T item);
	Task<IEnumerable<T>> GetByUserId(string customerId);
	Task<bool> DeleteItemAsync(string id);
}
