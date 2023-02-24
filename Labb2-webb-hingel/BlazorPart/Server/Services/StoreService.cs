using System.Runtime.Serialization;
using BlazorPart.Shared.DTOs;
using CustomerDataAccess.Models;
using MongoDB.Bson;
using StoreDataAccess.Models;
using StoreDataAccess.Repositories;

namespace BlazorPart.Server.Services;

//För att ta hand om affären, när listor måste samköras med kunder och ordrar.
//Uppdatera varor etc.
public class StoreService
{
	private readonly IStoreRepository<StoreProduct> _productStoreRepository;
	private readonly IStoreRepository<Order> _orderStoreRepository;
	private readonly CustomerService _customerService;
	private Order _activeOrder; //Hur sätta denna??
	private List<StoreProductDto> _shoppingCart = new (); //När skapas den?
	private StoreProductDto _activeProductDto;
	

	public StoreService(IStoreRepository<StoreProduct> productStoreRepository, CustomerService customerService, IStoreRepository<Order> orderStoreRepository)
	{
		_productStoreRepository = productStoreRepository;
		_customerService = customerService;
		_orderStoreRepository = orderStoreRepository;

	}

	public StoreProductDto ActiveProductDto
	{
		get => _activeProductDto;
		set => _activeProductDto = value;
	}


	//Hämta alla produkter, kan ju baka in kategorier och sådant här egentligen?
	public async Task<IEnumerable<StoreProductDto>> GetAllProducts()
	{
		//lägga på eventuella filter här i på något sätt
		//Kategorier
		//Eller ska allt ligga som separata metoder?
		//Kan ligga som if-satser om något är tomt eller liknande?
		//Checka om proukten är active eller inte?

		var products = await _productStoreRepository.GetAllItems();

		return products.Select(ConvertToDto);
	}

	public async Task AddNewProduct(StoreProductDto newDtoProduct)
	{
		var newProduct = new StoreProduct()
		{
			ProductName = newDtoProduct.ProductName,
			ProductDescription = newDtoProduct.ProductDescription,
			ProductType = newDtoProduct.ProductType,
			IsActive = true
		};

		await _productStoreRepository.AddItemAsync(newProduct);
	}

	public async Task<IEnumerable<StoreProductDto>> GetByName(string productName)
	{
		var products = await _productStoreRepository.GetItemByName(productName);

		return products.Select(ConvertToDto);
	}

	public async Task<IEnumerable<StoreProductDto>> GetById(string id)
	{
		var products =  await _productStoreRepository.GetItemByNumber(id);

		return products.Select(ConvertToDto);
	}

	public async Task DiscontinueItem(string productName)
	{
		var toUpdate = await _productStoreRepository.GetItemByName(productName);

		var productToUpdate = toUpdate.First();

		productToUpdate.IsActive = false;

		await _productStoreRepository.UpdateItem(productToUpdate);
	}

	private StoreProductDto ConvertToDto(StoreProduct product)
	{
		return new StoreProductDto()
		{
			ProductName = product.ProductName,
			ProductDescription = product.ProductDescription,
			ProductType = product.ProductType,
		};
	}

	//metoder:
	
	//Temp metod

	public async Task FillCart()
	{
		_shoppingCart.AddRange(await GetAllProducts());

		await PlaceOrder();
	}




	//Eller ska detta ligga i FrontEnd?
	public void AddProductToCart()
	{
		_shoppingCart.Add(_activeProductDto);
	}

	//Eller ska detta ligga i FrontEnd?
	public void RemoveProductFromCart()
	{
		_shoppingCart.Remove(_activeProductDto);
	}


	public async Task PlaceOrder()
	{
		if (_shoppingCart.Count > 0) //&& _customerService.ActiveCustomer != null)
		{
			var newOrder = new Order();
			newOrder.ProductDtos = _shoppingCart; //Select(ConvertProductToDto);
			newOrder.Customer = ConvertCustomerToDto(_customerService.ActiveCustomer);
			newOrder.OrderDate = DateTime.UtcNow;

			//Calculate sum:
			//Withdraw amount from account:
			//If ok:


			//TODO: Lägg till svarskod.
			await _orderStoreRepository.AddItemAsync(newOrder);
		}
	}

	private CustomerDto ConvertCustomerToDto(Customer activeCustomer)
	{
		return new CustomerDto()
		{
			FirstName = activeCustomer.FirstName,
			Email = activeCustomer.Email
			//Todo: Fyll på med mer info här som behövs.
		};
	}

	//private StoreProductDto ConvertProductToDto(StoreProduct product)
	//{
	//	return new StoreProductDto()
	//	{
	//		ProductDescription = product.ProductDescription,
	//		ProductName = product.ProductName,
	//		Price = product.Price,
	//		ProductType = product.ProductType
	//	};
	//}
}