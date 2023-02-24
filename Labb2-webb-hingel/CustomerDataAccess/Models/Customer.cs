using System.ComponentModel.DataAnnotations;

namespace CustomerDataAccess.Models;

public class Customer
{

	//TODO: Städa upp och sätt string.empty när testat klart. Lägg till övriga komponenter
	[Key]
	public Guid Id { get; set; } = Guid.NewGuid();
	public string FirstName { get; set; } = "not set"; 
	//public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = "hej@hej.se";
	//public string Phone { get; set; } = string.Empty;
	//public string StreetAdress { get; set; } = string.Empty;
	//public string City { get; set; } = string.Empty;
	//public string PostNumber { get; set; } = string.Empty;

	public string Password { get; set; } = "not set";
	//public byte[] PasswordSalt { get; set; }

}