﻿namespace BlazorPart.Shared.DTOs;

public class CustomerDto
{
	//denna ska typ ha samma som Customer, men inte lösenordsdelarna?
	public string FirstName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	//lägg till adress etc.
}