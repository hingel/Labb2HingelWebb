using Labb2HingelWebb.Shared;
using Labb2HingelWebb.Shared.DTOs;
using StoreDataAccess.Models;

namespace Labb2HingelWebb.Server.Services;

public class UnitOfWork
{
	private readonly PurchaseService _purchaseService;
	private readonly OrderService _orderService;

	public UnitOfWork(PurchaseService purchaseService, OrderService orderService)
	{
		_purchaseService = purchaseService;
		_orderService = orderService;
	}

	public async Task<ServiceResponse<string>> PlaceOrderCheck(OrderDto newOrderDto)
	{
		var placeOrderResult = await _orderService.PlaceOrder(newOrderDto);

		if (!placeOrderResult.Success)
		{
			return new ServiceResponse<string>()
			{
				Message = "Order not sent. Stock error.",
				Success = true
			};
		}

		var billingResult = await _purchaseService.PlaceOrder(newOrderDto);
		
		if (billingResult.Success)
		{
			return new ServiceResponse<string>()
			{
				Data = placeOrderResult.Data,
				Message = "Order sent",
				Success = true
			};
		}

		var result = await _orderService.DeleteOrderAsync(placeOrderResult.Data);

		if (result.Success)
		{
			return new ServiceResponse<string>()
			{
				Message = $"Order {placeOrderResult.Data} deleted successfully",
				Success = false
			};

		}

		return new ServiceResponse<string>()
		{
			Message = $"Order {placeOrderResult.Data} NOT deleted successfully",
			Success = false
		};
	}
}