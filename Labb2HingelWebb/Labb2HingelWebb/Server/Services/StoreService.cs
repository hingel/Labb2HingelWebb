using Labb2HingelWebb.Server.Models;
using Labb2HingelWebb.Shared.DTOs;
using StoreDataAccess.Models;
using StoreDataAccess.Repositories;

namespace Labb2HingelWebb.Server.Services;

//För att ta hand om affären, när listor måste samköras med kunder och ordrar.
//Uppdatera varor etc.
public class StoreService
{
	private readonly IStoreRepository<StoreProduct> _productStoreRepository;
	private readonly IStoreRepository<Order> _orderStoreRepository;
	private readonly CustomerService _customerService;
	private Order _activeOrder; //Hur sätta denna??
	private List<StoreProductDto> _shoppingCart = new(); //När skapas den? Kommer ursprungligen från FrontEnddelen
	private StoreProductDto _activeProductDto;


	public StoreService(IStoreRepository<StoreProduct> productStoreRepository, CustomerService customerService, IStoreRepository<Order> orderStoreRepository)
	{
		_productStoreRepository = productStoreRepository;
		_customerService = customerService;
		_orderStoreRepository = orderStoreRepository;

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
		var products = await _productStoreRepository.GetItemByNumber(id);

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

	public async Task PlaceOrder(OrderDto newOrderDto)
	{
		if (newOrderDto.ProductDtos != null) //&& _customerService.ActiveCustomer != null)
		{
			//Calculate sum:
			//Withdraw amount from account:
			//If ok:

			//Hitta namnet på användaren utifrån den info som skickas
			

			var newOrder = new Order()
			{
				CustomerDto = ConvertCustomerToDto(await _customerService.FindUserByName(newOrderDto.user)),
				ProductDtos = newOrderDto.ProductDtos,
				OrderDate = DateTime.UtcNow
			};

			//TODO: Lägg till svarskod.
			await _orderStoreRepository.AddItemAsync(newOrder);
		}
	}

	//private StoreProduct ConvertProductFromDto(StoreProductDto productDto)
	//{
	//	return new StoreProduct()
	//	{

	//	};
	//}

	private CustomerDto ConvertCustomerToDto(ApplicationUser activeCustomer)
	{
		return new CustomerDto()
		{
			UserName = activeCustomer.UserName,
			Email = activeCustomer.Email,
			Address = activeCustomer.Adress,
			Phone = activeCustomer.PhoneNumber
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

	//Temp metod


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
}