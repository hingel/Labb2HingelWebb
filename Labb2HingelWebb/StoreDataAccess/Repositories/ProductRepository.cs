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

	private async Task<bool> CheckProductExists(StoreProduct newProduct)
	{
		var filter = Builders<StoreProduct>.Filter.Eq("ProductName", newProduct.ProductName);
		var exists = await _storeProductCollection.FindAsync(filter);
		
		return exists.ToList().Count > 0; //exists is null && exists.ToList().Count > 0;
	}

	//TODO: ska jag göra till en GetAll och sen använda linq-uttryck? Men jobbigt att hämta hela databasen.
	public async Task<IEnumerable<StoreProduct>> GetItemByName(string productName)
	{
		var filter = Builders<StoreProduct>.Filter.StringIn("ProductName", productName); //TODO: Borde göra om detta.
		var products = await _storeProductCollection.FindAsync(filter);


		//TODO: Gör null check

		//return dtoProducts;
		return products.ToEnumerable();
	}


	//TODO: Fixa detta med id på ett bättre sätt. Ska jag lägga till en variabel till i objektet?
	public async Task<IEnumerable<StoreProduct>> GetItemByNumber(string id)
	{
		var filter = Builders<StoreProduct>.Filter.Eq("ProductId", id);
		var products = await _storeProductCollection.FindAsync(filter);

		//TODO: Gör null check

		//return dtoProducts;
		return products.ToEnumerable();
	}

	//Kunder nog vara en generell produktupdatering
	public async Task UpdateItem(StoreProduct updatedItem)
	{
		var filter = Builders<StoreProduct>.Filter.Eq("Id", updatedItem.Id);
		var update = Builders<StoreProduct>.Update.Set("ProductName", updatedItem.ProductName)
			.Set("ProductDescription", updatedItem.ProductDescription)
			.Set("ProductType", updatedItem.ProductType)
			.Set("IsActive", false);

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

	//TODO: Ta bort om det inte funkar att ha i andra filen
	//private StoreProductDto ConvertToDto(StoreProduct product)
	//{
	//	return new StoreProductDto()
	//	{
	//		ProductName = product.ProductName,
	//		ProductDescription = product.ProductDescription,
	//		ProductType = product.ProductType,
	//		IsActive = product.IsActive
	//	};
	//}

}