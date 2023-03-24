namespace StoreDataAccess.Repositories;

public interface IProductRepository<T>
{
	Task AddItemAsync(T item);
	Task<bool> UpdateItem(T item);
	Task<IEnumerable<T>> GetAllItems();
	Task<IEnumerable<T>> GetItemByName (string name);
	Task<long> DeleteItem(string name);
}