using Labb2HingelWebb.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace Labb2HingelWebb.Server.Services;

public class RoleService
{
	private readonly RoleManager<IdentityRole> _roleManager;

	public RoleService(RoleManager<IdentityRole> roleManager)
	{
		_roleManager = roleManager;
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