using Labb2HingelWebb.Server.Services;
using Labb2HingelWebb.Shared.DTOs;

namespace Labb2HingelWebb.Server.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplication MapCustomerEndPoints(this WebApplication app)
	{
		app.MapGet("/findUserByName/{name}", async (CustomerService customerService, string name) =>
			await customerService.FindUserByName(name));

		app.MapGet("/findCustomers", async (CustomerService customerService) => 
			await customerService.FindCustomers());

		app.MapPost("/updateUser", async (CustomerService customerService, CustomerDto updatedCustomerDto) => 
			await customerService.UpdateUser(updatedCustomerDto));

		return app;
	}

	public static WebApplication MapStoreEndPoints(this WebApplication app)
	{
		//TODO: snygga till det här:

		app.MapPost("/addStoreProduct",
			async (ProductService storeService, StoreProductDto newDtoProduct) =>
			{
				await storeService.AddNewProduct(newDtoProduct);
			});

		app.MapGet("/allProducts", async (ProductService storeService) =>
		{
			var response = await storeService.GetAllProducts();

			//return response.Data;
			return response.Success ? response.Data : null;
			//Resukt.OK(Response) : Result.BadRequest(Response)
			//TODO: ska jag skicka hela responset eller enbart datan?
		});

		app.MapDelete("/deleteProduct/{productName}", async (ProductService storeService, string productName) =>
			await storeService.DeleteProduct(productName));

		return app;
	}

	public static WebApplication MapOrderEndPoints(this WebApplication app)
	{
		app.MapPost("/placeOrder", async (OrderService orderService, OrderDto newOrder) =>
		{
			await orderService.PlaceOrder(newOrder);
		});

		app.MapGet("/getCustomerOrders/{email}", async (OrderService orderService, string email) =>
		{
			return await orderService.GetOrders(email);
		});
		
		return app;
	}
}