namespace StoreDataAccess.Repositories;

public interface IProductRepository<T>
{
	Task AddItemAsync(T item);
	Task<bool> UpdateItem(T item);
	Task<IEnumerable<T>> GetAllItems();
	Task<T> GetItemByName (string name);
	Task<T> GetItemById(string id);
	Task<long> DeleteItem(string name);
}