namespace StoreDataAccess.Repositories;

public interface IOrderRepository<T>
{	
	Task AddItemAsync(T item);
	Task<IEnumerable<T>> GetByEmail(string email);

}
