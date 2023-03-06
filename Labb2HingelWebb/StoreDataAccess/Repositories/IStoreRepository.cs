﻿namespace StoreDataAccess.Repositories;

public interface IStoreRepository<T>
{
	Task AddItemAsync(T item);
	Task UpdateItem(T item);
	Task<IEnumerable<T>> GetAllItems();
	Task<T> GetItemByName (string name);
	Task<T> GetItemByNumber(string id);

	//TODo: Lägg till ytterligare metoder här:
}