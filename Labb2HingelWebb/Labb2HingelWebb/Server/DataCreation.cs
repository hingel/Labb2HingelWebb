using Labb2HingelWebb.Server.Models;
using Labb2HingelWebb.Shared;
using Labb2HingelWebb.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using StoreDataAccess.Models;
using StoreDataAccess.Repositories;

namespace Labb2HingelWebb.Server;

public class DataCreation
{
	private readonly OrderRepository _orderRepository;
	private readonly ProductRepository _productRepository;
	public DataCreation(OrderRepository orderRepository, ProductRepository productRepository)
	{
		_orderRepository = orderRepository;
		_productRepository = productRepository;
	}

	public async Task AddDataAsync(ApplicationUser newUser)
	{
		//Först skapa produkter och lägga till dem.

		var espresso = new StoreProduct
		{
			ProductType = ProductCategory.Coffee,
			ProductDescription = "Single Espresso shot",
			Price = 20,
			ProductName = "Espresso",
			IsActive = true
		};

		await _productRepository.AddItemAsync(espresso);

		var cortado = new StoreProduct
		{
			ProductType = ProductCategory.Coffee,
			ProductDescription = "Cortado",
			Price = 20,
			ProductName = "Double Espresso with hot oat milk",
			IsActive = true
		};

		await _productRepository.AddItemAsync(cortado);

		var cappucino = new StoreProduct
		{
			ProductType = ProductCategory.Coffee,
			ProductDescription = "Cappucino",
			Price = 20,
			ProductName = "Single Espresso with hot and foamed oat milk",
			IsActive = true
		};

		await _productRepository.AddItemAsync(cappucino);

		var tea = new StoreProduct
		{
			ProductType = ProductCategory.Tea,
			ProductDescription = "Ceylon black tea",
			Price = 20,
			ProductName = "1 cup of prime Ceylon black tea",
			IsActive = true
		};

		await _productRepository.AddItemAsync(tea);

		var greenTea = new StoreProduct
		{
			ProductType = ProductCategory.Tea,
			ProductDescription = "Genmaicha",
			Price = 20,
			ProductName = "1 cup of prime green Japanese tea with roasted rice.",
			IsActive = false
		};

		await _productRepository.AddItemAsync(greenTea);

		var list = new List<ProductOrderQuantityDto>
		{
			new ProductOrderQuantityDto()
			{
				Quantity = 2,
				StoreProductDto = ConvertProductToDto(espresso)
			},

			new ProductOrderQuantityDto()
			{
				Quantity = 1,
				StoreProductDto = ConvertProductToDto(cortado)
			},

			new ProductOrderQuantityDto()
			{
				Quantity = 3,
				StoreProductDto = ConvertProductToDto(tea)
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
				LastName = newUser.LastName
			},
			OrderDate = DateTime.Now.AddDays(-5),
			ProductOrderQuantityDtos = list
		};
	
		await _orderRepository.AddItemAsync(newOrder);
	}

	private StoreProductDto ConvertProductToDto(StoreProduct product)
	{
		return new StoreProductDto()
		{
			ProductName = product.ProductName,
			ProductDescription = product.ProductDescription,
			ProductType = product.ProductType,
			Price = product.Price,
			IsActive = product.IsActive
		};
	}


}