using Labb2HingelWebb.Shared;
using Labb2HingelWebb.Shared.DTOs;
using StoreDataAccess.Models;
using StoreDataAccess.Repositories;

namespace Labb2HingelWebb.Server.Services;

public class OrderService
{
	private readonly IOrderRepository<Order> _orderStoreRepository;
	private readonly CustomerService _customerService;

	public OrderService(IOrderRepository<Order> orderStoreRepository, CustomerService customerService)
	{
		_orderStoreRepository = orderStoreRepository;
		_customerService = customerService;
	}

	public async Task<ServiceResponse<string>> PlaceOrder(OrderDto newOrderDto)
	{
		if (newOrderDto.ProductOrderQuantityDtos != null) //TODO: Flytta denna check till front end.
		{
			var response = await _customerService.FindUserByName(newOrderDto.UserName);

			if (response.Success)
			{
				var newOrder = new Order()
				{
					CustomerDto = response.Data,
					ProductOrderQuantityDtos = newOrderDto.ProductOrderQuantityDtos,
					OrderDate = DateTime.UtcNow
				};

				var ordernr = await _orderStoreRepository.AddItemAsync(newOrder);

				return new ServiceResponse<string>()
				{
					Data = ordernr,
					Message = "Order sent. Thank you!",
					Success = true
				};
			}
		}

		return new ServiceResponse<string>()
		{
			Message = "Order not sent.",
			Success = false
		};
	}

	public async Task<ServiceResponse<IEnumerable<OrderDto>>> GetOrders(string email)
	{
		var orders = await _orderStoreRepository.GetByEmail(email);

		if (orders != null && orders.Any()) //TODO: Vilken check behövs??
		{
			var response = new ServiceResponse<IEnumerable<OrderDto>>()
			{
				Data = orders.Select(o => new OrderDto()
				{
					OrderDate = o.OrderDate,
					Id = o.Id,
					ProductOrderQuantityDtos = o.ProductOrderQuantityDtos,
					Email = o.CustomerDto.Email,
					UserName = o.CustomerDto.UserName,
					Address = o.CustomerDto.Address
				}),
				Success = true,
				Message = "Order from ${email}"
			};

			return response;
		}

		return new ServiceResponse<IEnumerable<OrderDto>>()
		{
			Success = false,
			Message = "No orders found"
		};
	}

	public async Task<ServiceResponse<string>> DeleteOrderAsync(string id)
	{
		var result = await _orderStoreRepository.DeleteItemAsync(id);

		if (result)
		{
			return new ServiceResponse<string>()
			{
				Message = "Order deleted",
				Success = true
			};
		}

		return new ServiceResponse<string>()
		{
			Message = "Order not deleted, contact helpdesk",
			Success = false
		};
	}
}