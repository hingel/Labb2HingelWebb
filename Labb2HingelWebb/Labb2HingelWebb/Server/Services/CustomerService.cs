using System.Security.Claims;
using Labb2HingelWebb.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace Labb2HingelWebb.Server.Services;


//Tillfällig class för att fatta strukturen 
public class CustomerService
{
	//private readonly ICustomerRepository _customerRepo;
	private ApplicationUser _activeCustomer; //TODO: Eller ska det vara en Dto? //Customer heter något annat.
	private UserManager<ApplicationUser> _userManager;


	public CustomerService(UserManager<ApplicationUser> userManager)
	{
		_userManager = userManager;
	}


	//Flytta detta till någonannanstans eventuellt? Tror inte detta upplägget är så nödvändigt.
	public ApplicationUser ActiveCustomer
	{
		get => _activeCustomer;
		set => _activeCustomer = value;
	}


	//För att hitta en användare på servern
	public async Task<IdentityResult> FindUser(string email)
	{

		//var test = _userManager.Users.Where(u => u.Email.Contains(email)).FirstOrDefault();

		var test = await _userManager.FindByEmailAsync(email);

		test.UserName = "Stenen";

		var result = await _userManager.UpdateAsync(test);

		return result;



		//För att ta bort en användare
		//await _userManager.DeleteAsync(test);


		//För att lägga till claims
		await _userManager.AddClaimAsync(test, new Claim("hej", "hj"));

		//För att lägga till användaren till rollistor.
		await _userManager.AddToRoleAsync(test, "admin");
	}

	//TODO: Det ska generellt returneras CustomerDTOs härifrån till användaren.

}
