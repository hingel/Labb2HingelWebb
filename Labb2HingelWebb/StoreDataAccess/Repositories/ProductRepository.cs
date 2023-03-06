using MongoDB.Driver;
using StoreDataAccess.Models;

namespace StoreDataAccess.Repositories;

public class ProductRepository : IStoreRepository<StoreProduct>
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
			//TODO: Retunera svarsmeddelande
			return;
		}
		
		await _storeProductCollection.InsertOneAsync(newProduct);

		//ToDO: Fixa svarsmeddelande
	}

	//Är denna check tillförlitlig?
	private async Task<bool> CheckProductExists(StoreProduct newProduct)
	{
		var filter = Builders<StoreProduct>.Filter.Eq("ProductName", newProduct.ProductName);
		var exists = await _storeProductCollection.FindAsync(filter);
		
		return exists.ToList().Count > 0; //exists is null && exists.ToList().Count > 0;
	}

	//TODO: Ta hand om detta:
	public async Task<IEnumerable<StoreProduct>> GetByEmail(string email)
	{
		throw new NotImplementedException();
	}

	public async Task<StoreProduct> GetItemByName(string productName)
	{
		var filter = Builders<StoreProduct>.Filter.StringIn("ProductName", productName); //TODO: Borde göra om detta.
		var products = await _storeProductCollection.FindAsync(filter);


		//TODO: Gör null check

		return products.FirstOrDefault();
	}


	//TODO: Fixa detta med id på ett bättre sätt. Ska jag lägga till en variabel till i objektet?
	public async Task<StoreProduct> GetItemByNumber(string id)
	{
		var filter = Builders<StoreProduct>.Filter.Eq("ProductId", id);
		var products = await _storeProductCollection.FindAsync(filter);

		//TODO: Gör null check

		//return dtoProducts;
		return products.FirstOrDefault();
	}

	public async Task DeleteItem(string name)
	{
		var filter = Builders<StoreProduct>.Filter.Eq("ProductName", name);
		await _storeProductCollection.DeleteOneAsync(filter);

	}

	//Kunder nog vara en generell produktupdatering
	public async Task UpdateItem(StoreProduct updatedItem)
	{
		var filter = Builders<StoreProduct>.Filter.Eq("Id", updatedItem.Id);
		var update = Builders<StoreProduct>.Update.Set("ProductName", updatedItem.ProductName)
			.Set("ProductDescription", updatedItem.ProductDescription)
			.Set("ProductType", updatedItem.ProductType)
			.Set("IsActive", updatedItem.IsActive)
			.Set("Price", updatedItem.Price);

		await _storeProductCollection.FindOneAndUpdateAsync(filter, update,
			new FindOneAndUpdateOptions<StoreProduct, StoreProduct>() { IsUpsert = true });

		//TODO: Returnera ett svarsmeddelande också
	}

	public async Task<IEnumerable<StoreProduct>> GetAllItems()
	{
		var products = await _storeProductCollection.FindAsync(_ => true);

		//var dtoProducts = products.ToEnumerable().Select(ConvertToDto);

		//TODO: Gör null check??

		//return dtoProducts;
		return products.ToEnumerable();
	}
	
}