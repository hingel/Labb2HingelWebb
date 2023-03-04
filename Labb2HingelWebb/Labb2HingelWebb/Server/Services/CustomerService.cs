using Labb2HingelWebb.Server.Models;

namespace Labb2HingelWebb.Server.Services;


//Tillfällig class för att fatta strukturen 
public class CustomerService
{
	//private readonly ICustomerRepository _customerRepo;
	private ApplicationUser _activeCustomer; //TODO: Eller ska det vara en Dto? //Customer heter något annat.

	//public CustomerService(ICustomerRepository repository)
	//{
	//	var email = "hej@hej.com";

	//	_customerRepo = repository;
	//	_activeCustomer = _customerRepo.GetByEmail(email).Result; //TOdo: Tillfälligt test. Ta bort sen.
	//	//detta kan ske genom sättet jag gjorde i Testsidan i google projektet tänkte jag på något sätt.
	//}


	//Flytta detta till någonannanstans eventuellt?
	public ApplicationUser ActiveCustomer
	{
		get => _activeCustomer;
		set => _activeCustomer = value;
	}



	//TODO: Det ska generellt returneras CustomerDTOs härifrån till användaren.

}
