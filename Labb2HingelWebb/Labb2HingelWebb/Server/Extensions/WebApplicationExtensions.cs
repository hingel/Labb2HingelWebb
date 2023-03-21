using Azure;
using Labb2HingelWebb.Server.Services;
using Labb2HingelWebb.Shared;
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
		}).RequireAuthorization();

		app.MapGet("/findCustomers/{username}", async (CustomerService customerService, RoleService roleService, string username) =>
		{
			if (await roleService.UserIsInRole(username))
			{
				var response = await customerService.FindCustomers();
				return response.Success ? Results.Ok(response) : Results.BadRequest(response);
			}

			var response2 = new ServiceResponse<IEnumerable<CustomerDto>>
			{
				Message = "Not admin loggin",
				Success = false
			};

			return response2.Success ? Results.Ok(response2) : Results.BadRequest(response2);
			
		}).RequireAuthorization();

		app.MapPost("/updateUser", async (CustomerService customerService, CustomerDto updatedCustomerDto) =>
		{
			var response = await customerService.UpdateUser(updatedCustomerDto);

			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		}).RequireAuthorization();

		return app;
	}

	public static WebApplication MapStoreEndPoints(this WebApplication app)
	{
		app.MapPost("/addStoreProduct",
			async (ProductService storeService, StoreProductDto newDtoProduct) =>
			{
				var response = await storeService.AddNewProduct(newDtoProduct);

				return response.Success ? Results.Ok(response) : Results.BadRequest(response);
			}).RequireAuthorization();

		app.MapGet("/allProducts", async (ProductService storeService) =>
		{
			var response = await storeService.GetAllProducts();

			return response.Success ? Results.Ok(response) : Results.BadRequest(response);

		}).RequireAuthorization(); //"admin_access"); //Får inte detta att fungera.

		app.MapDelete("/deleteProduct/{productName}", async (ProductService storeService, string productName) =>
		{
			var response = await storeService.DeleteProduct(productName);

			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		}).RequireAuthorization("admin_access");

		return app;
	}

	public static WebApplication MapOrderEndPoints(this WebApplication app)
	{
		app.MapPost("/placeOrder", async (OrderService orderService, OrderDto newOrder) =>
		{
			var response = await orderService.PlaceOrder(newOrder);

			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		}).RequireAuthorization();

		app.MapGet("/getCustomerOrders/{email}", async (OrderService orderService, string email) =>
		{
			var response = await orderService.GetOrders(email);

			return response.Success ? Results.Ok(response) : Results.Ok(response); //TODO: Får inte tillbaks svar om inte OK resultat.
		}).RequireAuthorization();
		
		return app;
	}
}