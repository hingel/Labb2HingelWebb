using Microsoft.AspNetCore.Identity;

namespace Labb2HingelWebb.Server.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string? Adress { get; set; } = string.Empty;

		//TODO: Vet inte om denna ska vara med. Försök ta bort i databasen om det behövs?
		//public virtual ICollection<IdentityRole> Roles { get; set; } = new HashSet<IdentityRole>();

	}
}