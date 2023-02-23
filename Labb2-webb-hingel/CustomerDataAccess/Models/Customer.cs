using System.ComponentModel.DataAnnotations;

namespace CustomerDataAccess.Models;

public class Customer
{
	[Key]
	public Guid Id { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string StreetAdress { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string PostNumber { get; set; } = string.Empty;
	public byte[] Password {get; set; }
	public byte[] PasswordSalt { get; set; }

}