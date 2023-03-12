using Labb2HingelWebb.Shared.DTOs;

namespace Labb2HingelWebb.Shared.DTOs;

public class ProductOrderQuantityDto
{
	private int _quantity;

	public StoreProductDto StoreProductDto { get; set; } = new();
	public int Quantity
	{
		get => _quantity;
		set
		{
			_quantity = value;
			CalcSum();
		}
	}
	public int SumPrice { get; set; }

	public ProductOrderQuantityDto()
	{
		CalcSum();
	}

	private void CalcSum()
	{
		SumPrice = Quantity * StoreProductDto.Price;
	}
} 