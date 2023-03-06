namespace StoreDataAccess.Repositories;

public interface IStoreRepository<T>
{
	Task AddItemAsync(T item);
	Task UpdateItem(T item);
	Task<IEnumerable<T>> GetAllItems();
	Task<IEnumerable<T>> GetByEmail(string email);
	Task<T> GetItemByName (string name);
	Task<T> GetItemByNumber(string id);

	//TODo: Lägg till ytterligare metoder här:
	Task DeleteItem(string name);
}