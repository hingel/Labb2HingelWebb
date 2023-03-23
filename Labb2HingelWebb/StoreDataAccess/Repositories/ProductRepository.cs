using MongoDB.Driver;
using StoreDataAccess.Models;

namespace StoreDataAccess.Repositories;

public class ProductRepository : IProductRepository<StoreProduct>
{
	private readonly IMongoCollection<StoreProduct> _storeProductCollection;

	public ProductRepository()
	{
		var host = "localhost";
		var dataBaseName = "Lab2Web";
		var port = "27017";
		var connectionString = $"mongodb://{host}:{port}";
		var client = new MongoClient(connectionString);
		var dataBase = client.GetDatabase(dataBaseName);
		_storeProductCollection = dataBase.GetCollection<StoreProduct>("StoreProducts",
			new MongoCollectionSettings() { AssignIdOnInsert = true });
	}

	public async Task AddItemAsync(StoreProduct newProduct)
	{
		//Detta måsta fixas.
		if (await CheckProductExists(newProduct))
		{
			return;
		}
		
		await _storeProductCollection.InsertOneAsync(newProduct);
	}

	//Är denna check tillförlitlig?
	private async Task<bool> CheckProductExists(StoreProduct newProduct)
	{
		var filter = Builders<StoreProduct>.Filter.Eq("ProductName", newProduct.ProductName);
		var exists = await _storeProductCollection.FindAsync(filter);
		
		return exists.ToList().Count > 0;
	}
	
	public async Task<StoreProduct> GetItemByName(string productName)
	{
		var filter = Builders<StoreProduct>.Filter.StringIn("ProductName", productName);
		var products = await _storeProductCollection.FindAsync(filter);
		
		if (products == null)
		{
			return null;
		}

		return products.FirstOrDefault();
	}
	
	public async Task<StoreProduct> GetItemById(string id)
	{
		var filter = Builders<StoreProduct>.Filter.Eq("ProductId", id);
		var products = await _storeProductCollection.FindAsync(filter);

		if (products == null)
		{
			return null;
		}

		return products.FirstOrDefault();
	}

	public async Task<long> DeleteItem(string name)
	{
		var filter = Builders<StoreProduct>.Filter.Eq("ProductName", name);
		var result = await _storeProductCollection.DeleteOneAsync(filter);

		return result.DeletedCount;
	}

	public async Task<bool> UpdateItem(StoreProduct updatedItem)
	{
		var filter = Builders<StoreProduct>.Filter.Eq("Id", updatedItem.Id);
		var update = Builders<StoreProduct>.Update.Set("ProductName", updatedItem.ProductName)
			.Set("ProductDescription", updatedItem.ProductDescription)
			.Set("ProductType", updatedItem.ProductType)
			.Set("IsActive", updatedItem.IsActive)
			.Set("Price", updatedItem.Price);

		var result = await _storeProductCollection.FindOneAndUpdateAsync(filter, update,
			new FindOneAndUpdateOptions<StoreProduct, StoreProduct>() { IsUpsert = true });

		if (result != null)
		{
			return true;
		}

		return false;
	}
	
	public async Task<IEnumerable<StoreProduct>> GetAllItems()
	{
		var products = await _storeProductCollection.FindAsync(_ => true);
		
		return products.ToEnumerable();
	}
}