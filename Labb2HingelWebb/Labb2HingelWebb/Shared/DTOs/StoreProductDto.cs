﻿namespace Labb2HingelWebb.Shared.DTOs;

public class StoreProductDto
{
	public string ProductName { get; set; } = String.Empty;
	public string ProductDescription { get; set; } = String.Empty;
	public ProductCategory ProductType { get; set; } = ProductCategory.Other;
	public int Price { get; set; }
	public bool IsActive { get; set; }
}