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
	
	public async Task AddItemAsync(Order item)
	{
		item.Id = await GetOrderNumber();

		await _storeOrderCollection.InsertOneAsync(item);
	}

	public async Task<IEnumerable<Order>> GetByEmail(string email)
	{
		//var filter = Builders<Order.CusteomerDto>.Filter.Eq("CustomerDto", email); //TODO: ta hand om detta sorterar ut alla nu
		var result = await _storeOrderCollection.FindAsync(_ => true);

		return result.ToList().Where(o => o.CustomerDto.Email.ToLower() == email.ToLower());
	}

	//TODO: Denna är inte rikigt bra. Måste kolla om det kan finnas en order med det numret också.
	private async Task<string> GetOrderNumber()
	{
		var filter = Builders<Order>.Filter.Empty;
		var result = await _storeOrderCollection.CountDocumentsAsync(filter) + 1;
		return result.ToString();
	}
}