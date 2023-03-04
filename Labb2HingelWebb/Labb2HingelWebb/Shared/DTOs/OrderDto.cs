namespace Labb2HingelWebb.Shared.DTOs;

public class OrderDto
{
	public int Id { get; set; } //ToDO: Gör egen metod för detta. Finns inget inbyggt.
	public CustomerDto Customer { get; set; } //Vill inte spara lösenordsdelarna i denna del
	public IEnumerable<StoreProductDto> ProductDtos { get; set; } //Har bara det med nödvändiga nedsparat.
	public DateTime OrderDate { get; set; }
}