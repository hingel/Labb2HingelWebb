using Microsoft.AspNetCore.Identity;

namespace Labb2HingelWebb.Server.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string? Adress { get; set; } = string.Empty;
		//Kan jag lägga till ytterligare info här o köra en ny migration
	}
}