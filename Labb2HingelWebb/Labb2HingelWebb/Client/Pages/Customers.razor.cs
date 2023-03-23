using Labb2HingelWebb.Shared.DTOs;
using Labb2HingelWebb.Shared;
using System.Net.Http.Json;

namespace Labb2HingelWebb.Client.Pages;

partial class Customers
{
	public List<CustomerDto> CustomerDtos { get; set; } = new();
	public List<CustomerDto> SearchResult { get; set; } = new();
	public CustomerDto ActiveCustomer { get; set; } = new();
	public string EmailSearch { get; set; } = "";
	public List<OrderDto> CustomerOrders { get; set; } = new();
	public OrderDto ActiveOrder { get; set; } = new();
	public List<ProductOrderQuantityDto> ActiveListProducts { get; set; } = new();
	public string ResponseMessage { get; set; } = "";
	private int _orderSum = 0;
	private bool _isHidden = true;

	protected override async Task OnInitializedAsync()
	{
		var response = await HttpClient.GetFromJsonAsync<ServiceResponse<IEnumerable<CustomerDto>>>(HttpClient.BaseAddress + "findCustomers/");

		if (response.Success)
		{
			CustomerDtos = response.Data.ToList();
			ResponseMessage = response.Message;
			FilterResults();
		}
		else
			ResponseMessage = response.Message;

		await base.OnInitializedAsync();
	}

	private async Task SelectCustomer(CustomerDto customerDto)
	{
		ActiveCustomer = customerDto;

		var response = await HttpClient.GetFromJsonAsync<ServiceResponse<IEnumerable<OrderDto>>>(HttpClient.BaseAddress + $"getCustomerOrders/{customerDto.Email}");

		if (response.Success)
		{
			if (response.Data != null && response.Data.Any())
			{
				ResponseMessage = response.Message;
				CustomerOrders = response.Data.ToList();
			}
		}
		else
		{
			ResponseMessage = response.Message;
				CustomerOrders.Clear();
				_isHidden = true;
		}
	}

	private void FilterResults()
	{
		if (EmailSearch != "")
		{
			SearchResult = CustomerDtos.Where(c => c.Email.ToLower().Contains(EmailSearch.ToLower())).ToList();
		}

		else
		{
			SearchResult = CustomerDtos;
		}
	}

	private void SelectedOrder(OrderDto orderDto)
	{
		ActiveOrder = orderDto;
		_isHidden = false;
		ActiveListProducts = ActiveOrder.ProductOrderQuantityDtos.ToList();
		CalculateSum();
	}

	private void CalculateSum()
	{
		_orderSum = 0;

		foreach (var productOrderQuantity in ActiveListProducts)
		{
			_orderSum += productOrderQuantity.SumPrice;
		}
	}
}