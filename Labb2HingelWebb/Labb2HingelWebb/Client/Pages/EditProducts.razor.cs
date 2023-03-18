using Labb2HingelWebb.Shared.DTOs;
using Labb2HingelWebb.Shared;
using System.Net.Http.Json;

namespace Labb2HingelWebb.Client.Pages;

partial class EditProducts
{
	public StoreProductDto ActiveProductDto { get; set; } = new();
	public List<StoreProductDto> StoreProductDtos { get; set; } = new();
	public List<StoreProductDto> SearchResult { get; set; } = new();
	public string SearchWord { get; set; } = "";
	public int GenreSearch { get; set; } = 4;
	public string ResponseMessage { get; set; } = string.Empty;
	public int CategoryNumber { get; set; }
	public int NoOfResults { get; set; } = 5;

	protected override async Task OnInitializedAsync()
	{
		await GetProducts();

		await base.OnInitializedAsync();
	}

	private async Task GetProducts()
	{
		var response = await HttpClient.GetFromJsonAsync<ServiceResponse<StoreProductDto[]>>(HttpClient.BaseAddress + "allProducts");

		if (response.Success)
		{
			StoreProductDtos = response.Data.ToList();
			FilterResults();
			ResponseMessage = response.Message;
		}
		else
		{
			ResponseMessage = response.Message;
		}
	}

	private void FilterResults()
	{
		if (SearchWord != "")
		{
			SearchResult = StoreProductDtos.Where(p => p.ProductName.ToLower().Contains(SearchWord.ToLower())).Take(NoOfResults).ToList();
		}
		else
		{
			SearchResult = StoreProductDtos.Take(NoOfResults).ToList();
		}

		if (GenreSearch != 4)
		{
			SearchResult = SearchResult.Where(p => p.ProductType == (ProductCategory)GenreSearch).Take(NoOfResults).ToList();
		}
	}

	private void SelectProduct(StoreProductDto selectedProductDto)
	{
		ActiveProductDto = selectedProductDto;
		CategoryNumber = (int)selectedProductDto.ProductType;
	}

	private async Task UpdateProduct()
	{
		ActiveProductDto.ProductType = (ProductCategory)CategoryNumber;

		var response = await HttpClient.PostAsJsonAsync(HttpClient.BaseAddress + "addStoreProduct", ActiveProductDto);
		var result = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();

		if (result.Success)
		{
			ResponseMessage = result.Message;
			await GetProducts();
		}

		Console.WriteLine(ResponseMessage); //TODO: Ta bort detta efter test
	}

	private async Task DeleteProduct()
	{
		if (ActiveProductDto == null)
			return;

		var response = await HttpClient.DeleteAsync(HttpClient.BaseAddress + $"deleteProduct/{ActiveProductDto.ProductName}");
		var result = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();

		if (result.Success)
		{
			ResponseMessage = result.Message;
			await GetProducts();
		}

		Console.WriteLine(ResponseMessage); //TODO: Ta bort detta efter test

	}
}