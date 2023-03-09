using System.Security.Claims;
using Labb2HingelWebb.Client;
using Labb2HingelWebb.Server.Models;
using Labb2HingelWebb.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Labb2HingelWebb.Server.Services;


//Tillfällig class för att fatta strukturen 
public class CustomerService
{
	private UserManager<ApplicationUser> _userManager;


	public CustomerService(UserManager<ApplicationUser> userManager)
	{
		_userManager = userManager;
	}


	//public void TestCreate()
	//{
	//	var nUser = new ApplicationUser() { UserName = "test", Email = "test@test.se" };
	//	_userManager.CreateAsync(nUser, "Abcd123!");

	//	_userManager.AddToRoleAsync(nUser, "admin");
	//}

	//TODO: För test i nuläget.
	public async Task<IdentityResult> GetUserByEmail(string email)
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

	public async Task<IEnumerable<CustomerDto>> FindCustomers()
	{
		var result = _userManager.Users;
		
		//TODO: Lite konstigt med, borde finnas en async här
		
		return _userManager.Users.Select(ConvertCustomerToDto);
	}
	
	public CustomerDto ConvertCustomerToDto(ApplicationUser activeCustomer)
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
