using Labb2HingelWebb.Shared;
using Labb2HingelWebb.Shared.DTOs;
using StoreDataAccess.Models;

namespace Labb2HingelWebb.Server.Services;

public class PurchaseService
{


	public async Task<ServiceResponse<string>> PlaceOrder(OrderDto newOrderDto)
	{
		if (newOrderDto.ProductOrderQuantityDtos != null)
		{

				var newOrder = new Order()
				{
					CustomerDto = response.Data,
					ProductOrderQuantityDtos = newOrderDto.ProductOrderQuantityDtos,
					OrderDate = DateTime.UtcNow
				};

				await _orderStoreRepository.AddItemAsync(newOrder);

				return new ServiceResponse<string>()
				{
					Message = "Order sent. Thank you!",
					Success = true
				};
			
		}

		return new ServiceResponse<string>()
		{
			Message = "Order not sent.",
			Success = false
		};


		return new ServiceResponse<string>()
		{
			Message = "OK",
			Success = true
		};
	}
}