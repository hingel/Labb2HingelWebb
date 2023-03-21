namespace StoreDataAccess.Repositories;

public interface IOrderRepository<T>
{	
	Task<bool> AddItemAsync(T item);
	Task<IEnumerable<T>> GetByEmail(string email);
	Task<bool> DeleteItemAsync(T item);

}
