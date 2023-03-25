namespace Labb2HingelWebb.Shared.DTOs;

public class OrderDto
{
	public string Id { get; set; }
	public string Email { get; set; }
	public string UserName { get; set; }
	public string Address { get; set; }
	public IEnumerable<ProductOrderQuantityDto> ProductOrderQuantityDtos { get; set; }
	public DateTime OrderDate { get; set; }
}