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
	
	public async Task<bool> AddItemAsync(Order item)
	{
		item.Id = await GetOrderNumber();

		//TODO: Try catch är nog fel här: Använda CancellationToken istället från betalningen som check?
		try
		{
			//Dra bort produkter från lager om ok.
			await _storeOrderCollection.InsertOneAsync(item);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public async Task<IEnumerable<Order>> GetByEmail(string email)
	{
		//var filter = Builders<Order.CusteomerDto>.Filter.Eq("CustomerDto", email); //TODO: ta hand om detta sorterar ut alla nu
		var result = await _storeOrderCollection.FindAsync(_ => true);

		return result.ToList().Where(o => o.CustomerDto.Email.ToLower() == email.ToLower());
	}

	public async Task<bool> DeleteItemAsync(Order item)
	{
		var filter = Builders<Order>.Filter.Eq("Id", item.Id);
		var deleteResult = await _storeOrderCollection.DeleteOneAsync(filter);

		if (deleteResult.DeletedCount == 1)
		{
			return true;
		}

		return false;
	}

	private async Task<string> GetOrderNumber()
	{
		var filter = Builders<Order>.Filter.Empty;
		var result = await _storeOrderCollection.CountDocumentsAsync(filter) + 1;

		var checkOrderNo = Builders<Order>.Filter.Eq("Id",  result.ToString());
		while (await _storeOrderCollection.FindAsync(checkOrderNo) == null)
		{
			result += 1;
			checkOrderNo = Builders<Order>.Filter.Eq("Id", result);
		}
		
		return result.ToString();
	}
}