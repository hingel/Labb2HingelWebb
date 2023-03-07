using Labb2HingelWebb.Server.Services;
using Labb2HingelWebb.Shared.DTOs;

namespace Labb2HingelWebb.Server.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplication MapCustomerEndPoints(this WebApplication app)
	{
		//TODO: Gå igenom och kolla vilka som verkligen används!!!

		app.MapGet("/getUser", async (CustomerService customerService, string email) =>
			await customerService.FindUser(email));

		app.MapGet("/findUserByName/{name}", async (CustomerService customerService, string name) =>
			await customerService.FindUserByName(name));

		app.MapGet("/findCustomers", async (CustomerService customerService) => 
			await customerService.FindAllUsers());

		app.MapPost("/updateUser", async (CustomerService customerService, CustomerDto updatedCustomerDto) => 
			await customerService.UpdateUser(updatedCustomerDto));

		return app;
	}

	public static WebApplication MapStoreEndPoints(this WebApplication app)
	{
		//snygga till det här:

		app.MapPost("/addStoreProduct",
			async (StoreService storeService, StoreProductDto newDtoProduct) =>
			{
				await storeService.AddNewProduct(newDtoProduct);
			});

		app.MapGet("/getAllProducts", async (StoreService storeService) => await storeService.GetAllProducts());

		app.MapDelete("/deleteProduct/{productName}", async (StoreService storeService, string productName) =>
		{
			await storeService.DeleteProduct(productName);
		});

		app.MapGet("/getProductByName/{searchName}", async (StoreService storeService, string searchName) => await storeService.GetByName(searchName));


		//app.MapGet("/getProductByNumber", async (StoreService storeService, string id) => await storeService.GetById(id));
		//app.MapPut("/getDiscontinueProduct", async (StoreService storeService, string searchName) => await storeService.DiscontinueItem(searchName));


		return app;
	}
}