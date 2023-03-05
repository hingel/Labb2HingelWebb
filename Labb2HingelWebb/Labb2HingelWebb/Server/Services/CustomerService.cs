using System.Security.Claims;
using Labb2HingelWebb.Server.Models;
using Labb2HingelWebb.Shared.DTOs;
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


	//För att hitta en användare på servern och i nuläget uppdatera den
	public async Task<IdentityResult> FindUser(string email)
	{

		//var test = _userManager.Users.Where(u => u.Email.Contains(email)).FirstOrDefault();

		var test = await _userManager.FindByEmailAsync(email);

		test.UserName = "Stenen";
		test.Adress = "Paradisgatan 18, Gbg";

		var result = await _userManager.UpdateAsync(test);


		return result;



		//För att ta bort en användare
		//await _userManager.DeleteAsync(test);


		//För att lägga till claims
		await _userManager.AddClaimAsync(test, new Claim("hej", "hj"));

		//För att lägga till användaren till rollistor.
		await _userManager.AddToRoleAsync(test, "admin");
	}

	public async Task<CustomerDto> FindUserDtoByName(string name)
	{
		var user = await _userManager.FindByNameAsync(name);

		return new CustomerDto()
		{
			UserName = user.UserName,
			Email = user.Email,

			//Todo: Fyll på med mer info här som behövs.
		};
	}

	public async Task<ApplicationUser> FindUserByName(string nickName)
	{
		var user = await _userManager.FindByNameAsync(nickName);

		//Todo: Behövs felhantering

		return user;
	}

	//TODO: Det ska generellt returneras CustomerDTOs härifrån till användaren.

	public async Task<IdentityResult> UpdateUser(CustomerDto updatedCustomerDto)
	{
		var userToUpdate = await _userManager.FindByEmailAsync(updatedCustomerDto.Email);

		userToUpdate.Adress = updatedCustomerDto.Address;
		userToUpdate.PhoneNumber = updatedCustomerDto.Phone;
		userToUpdate.UserName = updatedCustomerDto.UserName;

		return await _userManager.UpdateAsync(userToUpdate);
	}
}
