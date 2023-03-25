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
		
		var data = products.Select(ConvertProductToDto);

		var test = new ServiceResponse<IEnumerable<StoreProductDto>>()
		{
			Data = data,
			Message = "Products found.",
			Success = true
		};

		return test;
	}

	public async Task<ServiceResponse<string>> AddNewProduct(StoreProductDto newDtoProduct)
	{
		var products = await _productRepository.GetAllItems();

		var toUpdate = products.FirstOrDefault(p => p.ProductName.ToLower().Equals(newDtoProduct.ProductName.ToLower()));

		if (toUpdate is not null )
		{
			toUpdate.ProductDescription = newDtoProduct.ProductDescription;
			toUpdate.ProductName = newDtoProduct.ProductName;
			toUpdate.IsActive = newDtoProduct.IsActive;
			toUpdate.Price = newDtoProduct.Price;
			toUpdate.ProductType = newDtoProduct.ProductType;

			var result = await _productRepository.UpdateItem(toUpdate);

			if (result)
			{
				return new ServiceResponse<string>()
				{
					Data = string.Empty,
					Message = "Product Updated",
					Success = true
				};
			}

			return new ServiceResponse<string>()
			{
				Data = string.Empty,
				Message = "Product NOT Updated",
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

	public async Task<ServiceResponse<string>> DeleteProduct(string productToDelete)
	{
		var numberOfDeletedProd = await _productRepository.DeleteItem(productToDelete);

		return new ServiceResponse<string>()
		{
			Message = $"{numberOfDeletedProd} {productToDelete} is deleted.",
			Success = true,
			Data = string.Empty
		};
	}

	public async Task<ServiceResponse<IEnumerable<StoreProductDto>>> GetProductByName(string productName)
	{
		var products = await _productRepository.GetItemByName(productName);

		var data = products.Select(ConvertProductToDto);

		var result = new ServiceResponse<IEnumerable<StoreProductDto>>()
		{
			Data = data,
			Message = "Products found.",
			Success = true
		};

		return result;
	}
}