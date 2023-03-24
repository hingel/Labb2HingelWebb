using Labb2HingelWebb.Server.Models;
using Labb2HingelWebb.Shared;
using Labb2HingelWebb.Shared.DTOs;
using StoreDataAccess.Models;
using StoreDataAccess.Repositories;

namespace Labb2HingelWebb.Server;

public class DataCreation
{
	private readonly IOrderRepository<Order> _orderRepository;
	private readonly IProductRepository<StoreProduct> _productRepository;
	public DataCreation(IOrderRepository<Order> orderRepository, IProductRepository<StoreProduct> productRepository)
	{
		_orderRepository = orderRepository;
		_productRepository = productRepository;
	}

	public async Task<ServiceResponse<List<string>>> AddDataAsync(ApplicationUser newUser)
	{
		var listAddedItems = new List<string>();

		var espresso = new StoreProduct
		{
			ProductType = ProductCategory.Coffee,
			ProductName = "Espresso",
			Price = 20,
			ProductDescription = "Single Espresso shot",
			IsActive = true
		};

		await _productRepository.AddItemAsync(espresso);
		listAddedItems.Add(espresso.ProductName);

		var cortado = new StoreProduct
		{
			ProductType = ProductCategory.Coffee,
			ProductName = "Cortado",
			Price = 20,
			ProductDescription = "Double Espresso with hot oat milk",
			IsActive = true
		};

		await _productRepository.AddItemAsync(cortado);
		listAddedItems.Add(cortado.ProductName);

		var cappucino = new StoreProduct
		{
			ProductType = ProductCategory.Coffee,
			ProductName = "Cappucino",
			Price = 20,
			ProductDescription = "Single Espresso with hot and foamed oat milk",
			IsActive = true
		};

		await _productRepository.AddItemAsync(cappucino);
		listAddedItems.Add(cappucino.ProductName);

		var tea = new StoreProduct
		{
			ProductType = ProductCategory.Tea,
			ProductName = "Ceylon black tea",
			Price = 20,
			ProductDescription = "1 cup of prime Ceylon black tea",
			IsActive = true
		};

		await _productRepository.AddItemAsync(tea);
		listAddedItems.Add(tea.ProductName);

		var greenTea = new StoreProduct
		{
			ProductType = ProductCategory.Tea,
			ProductName = "Genmaicha",
			Price = 20,
			ProductDescription = "1 cup of prime green Japanese tea with roasted rice.",
			IsActive = false
		};

		await _productRepository.AddItemAsync(greenTea);
		listAddedItems.Add(greenTea.ProductName);

		var list = new List<ProductOrderQuantityDto>
		{
			new ()
			{
				StoreProductDto = ConvertProductToDto(espresso),
				Quantity = 2,
			},

			new ()
			{
				StoreProductDto = ConvertProductToDto(cortado),
				Quantity = 1
			},

			new ()
			{
				StoreProductDto = ConvertProductToDto(tea),
				Quantity = 3
			}
		};
		
		var newOrder = new Order()
		{
			CustomerDto = new CustomerDto()
			{
				UserName = newUser.UserName,
				Email = newUser.Email,
				Address = newUser.Adress,
				Phone = newUser.PhoneNumber,
				FirstName = newUser.FirstName,
				LastName = newUser.LastName,
				CustomerId = newUser.Id
			},  
			OrderDate = DateTime.Now.AddDays(-5),
			ProductOrderQuantityDtos = list,
		};
	
		listAddedItems.Add($"Order with no: {await _orderRepository.AddItemAsync(newOrder)}");

		return new ServiceResponse<List<string>>()
		{
			Data = listAddedItems,
			Message = "Data added",
			Success = true
		};
	}

	private StoreProductDto ConvertProductToDto(StoreProduct product)
	{
		return new ()
		{
			ProductName = product.ProductName,
			ProductDescription = product.ProductDescription,
			ProductType = product.ProductType,
			Price = product.Price,
			IsActive = product.IsActive
		};
	}
}