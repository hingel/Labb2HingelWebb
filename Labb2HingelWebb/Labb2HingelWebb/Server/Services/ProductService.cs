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

	public async Task AddNewProduct(StoreProductDto newDtoProduct) //TODO: Returnera ett repspons
	{
		var products = await _productRepository.GetAllItems();

		if (products.Any(p => p.ProductName.ToLower().Equals(newDtoProduct.ProductName.ToLower()))) //TODO: Detta skulle kanske kunna bytas mot att köras i existerande funktion redan i repositoriet?
		{
			var toUpdate = await _productRepository.GetItemByName(newDtoProduct.ProductName);

			toUpdate.ProductDescription = newDtoProduct.ProductDescription;
			toUpdate.ProductName = newDtoProduct.ProductName;
			toUpdate.IsActive = newDtoProduct.IsActive;
			toUpdate.Price = newDtoProduct.Price;
			toUpdate.ProductType = newDtoProduct.ProductType;

			await _productRepository.UpdateItem(toUpdate);

			//TODO: Returnera svar härifrån om ok:
		}

		else
		{
			var newProduct = new StoreProduct()
			{
				ProductName = newDtoProduct.ProductName,
				ProductDescription = newDtoProduct.ProductDescription,
				ProductType = newDtoProduct.ProductType,
				IsActive = true,
				Price = newDtoProduct.Price
			};

			await _productRepository.AddItemAsync(newProduct);

			//TODO: Returnera svar härifrån om ok:
		}
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

	public async Task DeleteProduct(string productToDelete)
	{
		await _productRepository.DeleteItem(productToDelete);
	}
}