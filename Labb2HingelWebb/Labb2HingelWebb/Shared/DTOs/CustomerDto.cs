namespace Labb2HingelWebb.Shared.DTOs;

public class CustomerDto
{
	//denna ska typ ha samma som Customer, men inte lösenordsdelarna?
	public string UserName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;

	public string Phone { get; set; } = string.Empty;
	//lägg till övrigt etc.
}