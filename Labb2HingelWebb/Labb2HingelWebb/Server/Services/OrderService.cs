using Labb2HingelWebb.Server.Models;
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

	public async Task PlaceOrder(OrderDto newOrderDto)
	{
		if (newOrderDto.ProductDtos != null)
		{
			//Calculate sum:
			//Withdraw amount from account:
			//If ok:

			//Hitta namnet på användaren utifrån den info som skickas


			var newOrder = new Order()
			{
				CustomerDto = _customerService.ConvertCustomerToDto(await _customerService.FindUserByName(newOrderDto.UserName)),
				ProductDtos = newOrderDto.ProductDtos,
				OrderDate = DateTime.UtcNow
			};

			//TODO: Lägg till svarskod.
			await _orderStoreRepository.AddItemAsync(newOrder);
		}
	}

	public async Task<IEnumerable<OrderDto>> GetOrders(string email)
	{
		var orders = await _orderStoreRepository.GetByEmail(email);

		return orders.Select(o => new OrderDto()
		{
			OrderDate = o.OrderDate,
			Id = o.Id,
			ProductDtos = o.ProductDtos,
			Email = o.CustomerDto.Email,
			UserName = o.CustomerDto.UserName,
			Address = o.CustomerDto.Address
		});
	}
}