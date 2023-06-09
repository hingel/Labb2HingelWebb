﻿using System.Net.Http.Json;
using Labb2HingelWebb.Shared.DTOs;
using Labb2HingelWebb.Shared;
using Microsoft.AspNetCore.Components;

namespace Labb2HingelWebb.Client.Pages;

partial class StorePage : ComponentBase
{
	public List<ProductOrderQuantityDto> ShoppingCartDto { get; set; } = new();
	public List<StoreProductDto> ListStoreProductDtos { get; set; } = new();
	public string CustomerName { get; set; } = string.Empty;
	private string _responseMessage = string.Empty;
	private bool _showHide = false;
	private bool _show = true;
	private int _shoppingCartSum = 0;
	public string SearchText { get; set; } = string.Empty;

	protected override async Task OnInitializedAsync()
	{
		var userInfo = await AuthenticationStateProvider.GetAuthenticationStateAsync();
		
		CustomerName = userInfo.User.Identity.Name;

		var response = await HttpClient.GetFromJsonAsync<ServiceResponse<StoreProductDto[]>>(HttpClient.BaseAddress + "allProducts");

		if (response.Success)
		{
			ListStoreProductDtos.AddRange(response.Data.Where(p => p.IsActive));
			_responseMessage = response.Message;
		}

		await base.OnInitializedAsync();
	}

	public void AddProductToCart(StoreProductDto productDto)
	{
		var productToUpdate = ShoppingCartDto.FirstOrDefault(lsp => lsp.StoreProductDto.ProductName == productDto.ProductName);

		if (productToUpdate is null)
		{
			var productToAdd = new ProductOrderQuantityDto()
			{
				StoreProductDto = productDto,
				Quantity = 1
			};

			ShoppingCartDto.Add(productToAdd);
		}

		else
		{
			productToUpdate.Quantity += 1;
		}

		_shoppingCartSum += productDto.Price;
		_responseMessage = "Product Added";
	}

	private void RemoveProductFromCart(StoreProductDto productDto)
	{
		var productToUpdate = ShoppingCartDto.FirstOrDefault(lsp => lsp.StoreProductDto.ProductName == productDto.ProductName);

		if (productToUpdate is not null)
		{
			if (productToUpdate.Quantity > 1)
			{
				productToUpdate.Quantity -= 1;
			}

			else
			{
				ShoppingCartDto.Remove(productToUpdate);
			}

			_responseMessage = "Product Removed";
			_shoppingCartSum -= productDto.Price;
		}
	}

	private async Task CheckOut()
	{
		if (ShoppingCartDto.Count == 0)
		{
			return;
		}

		var userInfo = await AuthenticationStateProvider.GetAuthenticationStateAsync();
		var user = userInfo.User.Identity.Name;

		var newOrder = new OrderDto()
		{
			ProductOrderQuantityDtos = ShoppingCartDto,
			UserName = user
		};

		var response = await HttpClient.PostAsJsonAsync(HttpClient.BaseAddress + "placeOrder", newOrder);
		var result = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();
		
		ShoppingCartDto = new();
		_responseMessage = result.Message;
		
		_shoppingCartSum = 0;
	}

	private async Task SearchProduct()
	{
		if (SearchText.Length != 0)
		{
			var response = await HttpClient.GetFromJsonAsync<ServiceResponse<StoreProductDto[]>>(HttpClient.BaseAddress + $"getProductByName/{SearchText}");

			if (response.Success)
			{
				ListStoreProductDtos.Clear();
				ListStoreProductDtos.AddRange(response.Data.Where(p => p.IsActive));
				_responseMessage = response.Message;
			}
		}
	}
}