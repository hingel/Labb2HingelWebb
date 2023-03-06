using Labb2HingelWebb.Server.Models;
using Labb2HingelWebb.Shared.DTOs;
using StoreDataAccess.Models;
using StoreDataAccess.Repositories;

namespace Labb2HingelWebb.Server.Services;

//För att ta hand om affären, när listor måste samköras med kunder och ordrar.
//Uppdatera varor etc.

//TODO: denna borde nog delas i två delar för att inte blanda ihop för mycket? Single Responsibility
public class StoreService
{
	private readonly IStoreRepository<StoreProduct> _productStoreRepository;
	private readonly IStoreRepository<Order> _orderStoreRepository;
	private readonly CustomerService _customerService;

	public StoreService(IStoreRepository<StoreProduct> productStoreRepository, CustomerService customerService, IStoreRepository<Order> orderStoreRepository)
	{
		_productStoreRepository = productStoreRepository;
		_customerService = customerService;
		_orderStoreRepository = orderStoreRepository;
	}
	
	//Hämta alla produkter, kan ju baka in kategorier och sådant här egentligen?
	public async Task<IEnumerable<StoreProductDto>> GetAllProducts()
	{
		var products = await _productStoreRepository.GetAllItems();

		return products.Select(ConvertProductToDto);
	}

	public async Task AddNewProduct(StoreProductDto newDtoProduct) //TODO: Returnera ett repspons
	{
		//Först kolla om produkten med samma namn redan existerar:

		var products = await _productStoreRepository.GetAllItems();

		if (products.Any(p => p.ProductName.ToLower().Equals(newDtoProduct.ProductName.ToLower()))) //TODO: Detta skulle kanske kunna bytas mot att köras i existerande funktion redan i repositoriet?
		{
			var toUpdate = await _productStoreRepository.GetItemByName(newDtoProduct.ProductName);

			toUpdate.ProductDescription = newDtoProduct.ProductDescription;
			toUpdate.ProductName = newDtoProduct.ProductName;
			toUpdate.IsActive = newDtoProduct.IsActive;
			toUpdate.Price = newDtoProduct.Price;
			toUpdate.ProductType = newDtoProduct.ProductType;

			await _productStoreRepository.UpdateItem(toUpdate);

			//Returnera svar härifrån om ok:
		}

		else
		{
			var newProduct = new StoreProduct()
			{
				ProductName = newDtoProduct.ProductName,
				ProductDescription = newDtoProduct.ProductDescription,
				ProductType = newDtoProduct.ProductType,
				IsActive = true
			};

			await _productStoreRepository.AddItemAsync(newProduct);

			//Returnera svar härifrån om ok:
		}
	}

	public async Task<StoreProductDto> GetByName(string productName)
	{
		var product = await _productStoreRepository.GetItemByName(productName);

		return ConvertProductToDto(product);
	}

	public async Task<StoreProductDto> GetById(string id)
	{
		var product = await _productStoreRepository.GetItemByNumber(id);

		return ConvertProductToDto(product);
	}

	//Borde inte behövas, kör allt genom uppdateringsmetoden istället.
	public async Task DiscontinueItem(string productName)
	{
		var toUpdate = await _productStoreRepository.GetItemByName(productName);
		
		toUpdate.IsActive = false;

		await _productStoreRepository.UpdateItem(toUpdate);
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


	//Todo: Denna kanske inte borde ligga här i eftersom den tar hand om kundgrejer. Ligga i customer service
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
}