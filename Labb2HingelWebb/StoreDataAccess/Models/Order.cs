using Labb2HingelWebb.Shared.DTOs;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreDataAccess.Models;

public class Order
{
	[BsonId] 
	public int Id { get; set; } //ToDO: Gör egen metod för detta. Finns inget inbyggt.
	
	[BsonElement]
	public CustomerDto Customer { get; set; } //Vill inte spara lösenordsdelarna i denna del. Eller bara Email?

	[BsonElement]
	public IEnumerable<StoreProductDto> ProductDtos { get; set; } //Eller ska detta vara Dtos för att tex inte spara bilder?
	
	[BsonElement]
	public DateTime OrderDate { get; set; }

}