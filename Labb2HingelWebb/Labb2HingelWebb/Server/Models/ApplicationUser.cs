using Microsoft.AspNetCore.Identity;

namespace Labb2HingelWebb.Server.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string? Adress { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
	}
}