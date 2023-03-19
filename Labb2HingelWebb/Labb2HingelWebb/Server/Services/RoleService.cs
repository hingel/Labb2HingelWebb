using Labb2HingelWebb.Server.Data;
using Labb2HingelWebb.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace Labb2HingelWebb.Server.Services;

public class RoleService
{
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly UserManager<ApplicationUser> _userManager;


	public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
	{
		_roleManager = roleManager;
		_userManager = userManager;

	}


	public async Task<bool> UserIsInRole(string username)
	{
		var user = await _userManager.FindByNameAsync(username);
	
		return await _userManager.IsInRoleAsync(user, "admin");
	}

	//detta borde skapa en ny roll som sen kan läggas till e
	public async Task CreateRole()
	{
		var newRole = new IdentityRole();

		newRole.Name = "admin";

		_roleManager.CreateAsync(newRole);

	}

	public async Task DeleteRole()
	{

	}
}