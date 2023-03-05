using Labb2HingelWebb.Shared;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreDataAccess.Models;

public class StoreProduct
{
	[BsonId] 
	public ObjectId Id { get; set; }
	[BsonElement]
	public string ProductName { get; set; } = String.Empty;
	[BsonElement]
	public string ProductDescription { get; set; } = String.Empty;
	[BsonElement]
	public ProductCategory ProductType { get; set; } = ProductCategory.None;
	[BsonElement]
	public bool IsActive { get; set; }
	[BsonElement]
	public int Price { get; set; }

	//Denna kan jag ta bort sen:
	[BsonElement]
	public string PictureLink { get; set; }
	//lägg till ytterligare 
}