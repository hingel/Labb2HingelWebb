using Labb2HingelWebb.Server.Models;
using Labb2HingelWebb.Shared;
using Labb2HingelWebb.Shared.DTOs;
using StoreDataAccess.Models;
using StoreDataAccess.Repositories;

namespace Labb2HingelWebb.Server.Services;

public class ProductService
{
	private readonly IProductRepository<StoreProduct> _productRepository;
	private readonly CustomerService _customerService;

	public ProductService(IProductRepository<StoreProduct> productRepository, CustomerService customerService)
	{
		_productRepository = productRepository;
		_customerService = customerService;
	}

	public async Task<ServiceResponse<IEnumerable<StoreProductDto>>> GetAllProducts()
	{
		var products = await _productRepository.GetAllItems();
		
		if (products != null)
		{
			var data = products.Select(ConvertProductToDto);

			var test = new ServiceResponse<IEnumerable<StoreProductDto>>()
			{
				Data = data,
				Message = "Products",
				Success = true
			};

			return test;
		}

		return new ServiceResponse<IEnumerable<StoreProductDto>>()
		{
			Message = "No products found",
			Success = false
		};
	}

	public async Task<ServiceResponse<string>> AddNewProduct(StoreProductDto newDtoProduct)
	{
		var products = await _productRepository.GetAllItems();

		if (products.Any(p => p.ProductName.Equals(newDtoProduct.ProductName)))
		{
			var toUpdate = await _productRepository.GetItemByName(newDtoProduct.ProductName);

			toUpdate.ProductDescription = newDtoProduct.ProductDescription;
			toUpdate.ProductName = newDtoProduct.ProductName;
			toUpdate.IsActive = newDtoProduct.IsActive;
			toUpdate.Price = newDtoProduct.Price;
			toUpdate.ProductType = newDtoProduct.ProductType;

			await _productRepository.UpdateItem(toUpdate);

			return new ServiceResponse<string>()
			{
				Data = string.Empty,
				Message = "Product Updated",
				Success = true
			};
		}

		var newProduct = new StoreProduct()
		{
			ProductName = newDtoProduct.ProductName,
			ProductDescription = newDtoProduct.ProductDescription,
			ProductType = newDtoProduct.ProductType,
			IsActive = true,
			Price = newDtoProduct.Price
		};
		await _productRepository.AddItemAsync(newProduct);

		return new ServiceResponse<string>()
		{
			Data = string.Empty,
			Message = "Product Added",
			Success = true
		};
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

	//TODO: Ska jag ta hand om mer alternativ om produkten inte hittas tex? Men ska vara väldigt låg risk.
	public async Task<ServiceResponse<string>> DeleteProduct(string productToDelete)
	{
		await _productRepository.DeleteItem(productToDelete);

		return new ServiceResponse<string>()
		{
			Message = $"{productToDelete} is deleted.",
			Success = true,
			Data = string.Empty
		};
	}
}