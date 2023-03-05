using Labb2HingelWebb.Shared.DTOs;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreDataAccess.Models;

public class Order
{
	[BsonId] 
	public string Id { get; set; }

	[BsonElement]
	public CustomerDto CustomerDto { get; set; } //Vill inte spara lösenordsdelarna i denna del.

	[BsonElement]
	public IEnumerable<StoreProductDto> ProductDtos { get; set; }
	
	[BsonElement]
	public DateTime OrderDate { get; set; }

}