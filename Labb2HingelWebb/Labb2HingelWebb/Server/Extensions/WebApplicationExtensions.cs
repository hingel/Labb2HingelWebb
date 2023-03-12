using Azure;
using Labb2HingelWebb.Server.Services;
using Labb2HingelWebb.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Labb2HingelWebb.Server.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplication MapCustomerEndPoints(this WebApplication app)
	{
		app.MapGet("/findUserByName/{name}", async (CustomerService customerService, string name) =>
		{
			var response = await customerService.FindUserByName(name);

			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		});

		app.MapGet("/findCustomers", async (CustomerService customerService) =>
		{
			var response = await customerService.FindCustomers();

			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		});

		app.MapPost("/updateUser", async (CustomerService customerService, CustomerDto updatedCustomerDto) =>
		{
			var response = await customerService.UpdateUser(updatedCustomerDto);

			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		});

		return app;
	}

	public static WebApplication MapStoreEndPoints(this WebApplication app)
	{
		app.MapPost("/addStoreProduct",
			async (ProductService storeService, StoreProductDto newDtoProduct) =>
			{
				var response = await storeService.AddNewProduct(newDtoProduct);

				return response.Success ? Results.Ok(response) : Results.BadRequest(response);
			});

		app.MapGet("/allProducts", async (ProductService storeService) =>
		{
			var response = await storeService.GetAllProducts();

			return response.Success ? Results.Ok(response) : Results.BadRequest(response);

		}); //.RequireAuthorization("AdminAccess");

		app.MapDelete("/deleteProduct/{productName}", async (ProductService storeService, string productName) =>
		{
			var response = await storeService.DeleteProduct(productName);

			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		});

		return app;
	}

	public static WebApplication MapOrderEndPoints(this WebApplication app)
	{
		app.MapPost("/placeOrder", async (OrderService orderService, OrderDto newOrder) =>
		{
			var response = await orderService.PlaceOrder(newOrder);

			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		});

		app.MapGet("/getCustomerOrders/{email}", async (OrderService orderService, string email) =>
		{
			var response = await orderService.GetOrders(email);

			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		});
		
		return app;
	}
}