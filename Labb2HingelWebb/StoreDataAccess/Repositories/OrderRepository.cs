using MongoDB.Driver;
using StoreDataAccess.Models;

namespace StoreDataAccess.Repositories;

public class OrderRepository : IStoreRepository<Order>
{
	private readonly IMongoCollection<Order> _storeOrderCollection;

	public OrderRepository()
	{
		var host = "localhost";
		var dataBaseName = "Lab2Web";
		var port = "27017";
		var connectionString = $"mongodb://{host}:{port}";
		var client = new MongoClient(connectionString);
		var dataBase = client.GetDatabase(dataBaseName);
		_storeOrderCollection = dataBase.GetCollection<Order>("OrdersCollection",
			new MongoCollectionSettings() { AssignIdOnInsert = true });
	}


	public async Task AddItemAsync(Order item)
	{
		item.Id = await GetOrderNumber();

		await _storeOrderCollection.InsertOneAsync(item);
	}

	public async Task UpdateItem(Order item)
	{
		throw new NotImplementedException();
	}

	public async Task<IEnumerable<Order>> GetAllItems()
	{
		throw new NotImplementedException();
	}


	public async Task<IEnumerable<Order>> GetByEmail(string email)
	{
		//var filter = Builders<Order.CusteomerDto>.Filter.Eq("CustomerDto", email); //TODO: ta hand om detta?
		var result = await _storeOrderCollection.FindAsync(_ => true);

		return result.ToList().Where(o => o.CustomerDto.Email.ToLower() == email.ToLower());
	}

	public async Task<Order> GetItemByName(string name)
	{
		throw new NotImplementedException();
	}

	public async Task<Order> GetItemByNumber(string id)
	{
		throw new NotImplementedException();
	}

	public async Task DeleteItem(string name)
	{
		throw new NotImplementedException();
	}

	private async Task<string> GetOrderNumber()
	{
		var filter = Builders<Order>.Filter.Empty;
		var result = await _storeOrderCollection.CountDocumentsAsync(filter) + 1;
		return result.ToString();
	}
}