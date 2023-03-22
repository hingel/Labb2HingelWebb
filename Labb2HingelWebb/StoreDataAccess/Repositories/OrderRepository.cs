using MongoDB.Driver;
using StoreDataAccess.Models;

namespace StoreDataAccess.Repositories;

public class OrderRepository : IOrderRepository<Order>
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
	
	public async Task<string> AddItemAsync(Order item)
	{
		item.Id = await GetOrderNumber();

		await _storeOrderCollection.InsertOneAsync(item);

		return item.Id;
	}

	public async Task<IEnumerable<Order>> GetByEmail(string email)
	{
		//var filter = Builders<Order.CusteomerDto>.Filter.Eq("CustomerDto", email); //TODO: ta hand om detta sorterar ut alla nu
		var result = await _storeOrderCollection.FindAsync(_ => true);

		return result.ToList().Where(o => o.CustomerDto.Email.ToLower() == email.ToLower());
	}
	
	public async Task<bool> DeleteItemAsync(string id)
	{
		var filter = Builders<Order>.Filter.Eq("Id", id);
		var test = await _storeOrderCollection.DeleteOneAsync(filter);

		if (test.DeletedCount == 1)
		{
			return true;
		}

		return false;
	}

	private async Task<string> GetOrderNumber()
	{
		var filter = Builders<Order>.Filter.Empty;
		var result = await _storeOrderCollection.CountDocumentsAsync(filter) + 1;

		var checkOrderNo = Builders<Order>.Filter.Eq("Id", result.ToString());
		while (await _storeOrderCollection.FindAsync(checkOrderNo) == null)
		{
			result += 1;
			checkOrderNo = Builders<Order>.Filter.Eq("Id", result);
		}

		return result.ToString();
	}
}