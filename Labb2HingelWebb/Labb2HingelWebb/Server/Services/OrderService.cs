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
		if (newOrderDto.ProductOrderQuantityDtos == null)
		{
			return new ServiceResponse<string>()
			{
				Message = "Order not sent. Nor products added.",
				Success = false
			};
		}

		var response = await _customerService.FindUserByName(newOrderDto.UserName);

		if (response.Success)
		{
			var newOrder = new Order()
			{
				CustomerDto = response.Data,
				ProductOrderQuantityDtos = newOrderDto.ProductOrderQuantityDtos,
				OrderDate = DateTime.UtcNow
			};

			var addedProductCheck = await _orderStoreRepository.AddItemAsync(newOrder);
			var paymentCheck = false;

			if (addedProductCheck)
			{
				//Betalningssimulering
				var randomPayment = new Random();

				if (randomPayment.Next(0, 10) < 7)
				{
					paymentCheck = true;
				}
			}

			if (!paymentCheck)
			{
				var deleteProductCheck = await _orderStoreRepository.DeleteItemAsync(newOrder);

				if (!deleteProductCheck)
				{
					//TODO: Har inget id här hur göra det bättre
					throw new Exception($"Order: {newOrder.Id}, {newOrder.OrderDate} coudn't be removed. Contact support.");
					//Kanske borde försöka igen?
				}

				return new ServiceResponse<string>()
				{
					Message = "Order not sent. Payment failed",
					Success = true
				};
			}

			//Send order to dispatch department.

			return new ServiceResponse<string>()
			{
				Message = "Order sent. Thank you!",
				Success = true
			};
		}

		return new ServiceResponse<string>()
		{
			Message = "Order not sent. No User found.",
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
}