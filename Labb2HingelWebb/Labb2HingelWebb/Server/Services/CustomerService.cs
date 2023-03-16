using System.Security.Claims;
using Labb2HingelWebb.Client;
using Labb2HingelWebb.Server.Data;
using Labb2HingelWebb.Server.Models;
using Labb2HingelWebb.Shared;
using Labb2HingelWebb.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Labb2HingelWebb.Server.Services;


//Tillfällig class för att fatta strukturen 
public class CustomerService
{
	private UserManager<ApplicationUser> _userManager;
	private IHttpContextAccessor _contextAccessor;
	private readonly ApplicationDbContext _appDbcontext;


	public CustomerService(UserManager<ApplicationUser> userManager, IHttpContextAccessor accessor, ApplicationDbContext appDbContext )
	{
		_userManager = userManager;
		_contextAccessor = accessor;
		_appDbcontext = appDbContext;
	}

	public async Task<ServiceResponse<string>> CheckRole()
	{
		var user =  _contextAccessor.HttpContext.User;

		var check = user.IsInRole("admin");

		//var checkRole =
		//	await _appDbcontext.UserRoles.AnyAsync(
		//		user.IsInRole(_appDbcontext.Roles.FirstOrDefault(r => r.Name == "admin").Name));

		if (check)
		{
			return new ServiceResponse<string>()
			{
				Data = "hittad",
				Message = "admin användare",
				Success = true
			};
		}

		return new ServiceResponse<string>()
		{

		}

	}

	//TODO: För test i nuläget. Används inte ta bort
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

	public async Task<ServiceResponse<CustomerDto>> FindUserByName(string nickName)
	{
		var user = await _userManager.FindByNameAsync(nickName);

		if (user != null)
		{
			return new ServiceResponse<CustomerDto>()
			{
				Data = ConvertCustomerToDto(user),
				Message = $"{nickName} found",
				Success = true
			};
		}

		return new ServiceResponse<CustomerDto>()
		{
			Message = $"{nickName} not found",
			Success = false
		};
	}

	//TODO: Det ska generellt returneras CustomerDTOs härifrån till användaren.

	public async Task<ServiceResponse<CustomerDto>> UpdateUser(CustomerDto updatedCustomerDto)
	{
		//TODO: Fråga till Niklas, ska detta läggas i en trycatch för att inte få exception??
		var userToUpdate = await _userManager.FindByEmailAsync(updatedCustomerDto.Email);

		userToUpdate.Adress = updatedCustomerDto.Address;
		userToUpdate.PhoneNumber = updatedCustomerDto.Phone;
		userToUpdate.UserName = updatedCustomerDto.UserName;

		var response = await _userManager.UpdateAsync(userToUpdate);

		if (response.Succeeded)
		{
			return new ServiceResponse<CustomerDto>()
			{
				Data = ConvertCustomerToDto(userToUpdate),
				Message = "User updated",
				Success = true
			};
		}

		return new ServiceResponse<CustomerDto>()
		{
			Message = "User not updated",
			Success = false
		};
	}

	public async Task<ServiceResponse<IEnumerable<CustomerDto>>> FindCustomers()
	{
		//TODO: Lite konstigt med, borde finnas en async här
		var result = _userManager.Users;

		if (result != null)
		{
			return new ServiceResponse<IEnumerable<CustomerDto>>()
			{
				Data = result.Select(ConvertCustomerToDto),
				Message = "Current users",
				Success = true
			};
		}

		return new ServiceResponse<IEnumerable<CustomerDto>>()
		{
			Message = "No users found",
			Success = false
		};
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
