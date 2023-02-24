using BlazorPart.Shared.DTOs;
using CustomerDataAccess.Context;
using CustomerDataAccess.Models;
using CustomerDataAccess.Repositories;

namespace BlazorPart.Server.Services;

//Tillfällig class för att fatta strukturen 
public class CustomerService
{
	private readonly ICustomerRepository _customerRepo;
	private Customer _activeCustomer; //TODO: Eller ska det vara en Dto?

	public CustomerService(ICustomerRepository repository)
	{
		var email = "hej@hej.com";

		_customerRepo = repository;
		_activeCustomer = _customerRepo.GetByEmail(email).Result; //TOdo: Tillfälligt test. Ta bort sen.
	}

	public Customer ActiveCustomer
	{
		get => _activeCustomer; 
		set => _activeCustomer = value;
	}

	public async Task LogIn()
	{
		//TODO: Här metod för att logga in på något sätt.
		//Eller om den ska ligga i DataAccessen? Eventuellt en metod där beroende på hur processen ser ut.
	}

	public async Task AddCustomer(Customer customer)
	{
		if (await _customerRepo.PostNewCustomer(customer))
		{
			_activeCustomer = customer;
		}

		//Om inloggning gått bra följande:
		

		//TODO: Returnera svarsmeddelanden
	}


	//TODO: Det ska generellt returneras CustomerDTOs härifrån till användaren.

}
