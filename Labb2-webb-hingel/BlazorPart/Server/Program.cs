using BlazorPart.Server.Services;
using BlazorPart.Shared.DTOs;
using CustomerDataAccess.Context;
using CustomerDataAccess.Models;
using CustomerDataAccess.Repositories;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using StoreDataAccess.Models;
using StoreDataAccess.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();



builder.Services.AddDbContext<CustomerContext>(options =>
{
	var connectionString = builder.Configuration.GetConnectionString("CustomersDB");
	options.UseSqlServer(connectionString);
});

//Customers
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<CustomerService>();

//StoreProducts
builder.Services.AddScoped<IStoreRepository<StoreProduct>, ProductRepository>();

//Ska denna vara singleton??
builder.Services.AddScoped<StoreService>();
builder.Services.AddScoped<IStoreRepository<Order>, OrderRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

//Customer APIs
app.MapPost("/addCustomer", async (CustomerService customerService, CustomerDto newCustomerDTo) =>
{
	var newCustomer = new Customer() { FirstName = newCustomerDTo.FirstName, Email = newCustomerDTo.Email };
	await customerService.AddCustomer(newCustomer);
});

//StoreItem APIs
app.MapPost("/addStoreProduct",
	async (StoreService storeService, StoreProductDto newDtoProduct) =>
	{
		await storeService.AddNewProduct(newDtoProduct);
	});

app.MapGet("/getAllProducts", async (StoreService storeService) => await storeService.GetAllProducts());
app.MapGet("/getProductByName", async (StoreService storeService, string searchName) => await storeService.GetByName(searchName));
app.MapGet("/getProductByNumber", async (StoreService storeService, string id) => await storeService.GetById(id));
app.MapPut("/getDiscontinueProduct", async (StoreService storeService, string searchName) => await storeService.DiscontinueItem(searchName));



//Orders APIs, de mesta är för admin, måste kolla att vem som helst inte kan köra dessa:

app.MapPost("/placeOrder", async (StoreService storeService) => await storeService.PlaceOrder());
//Få tillbaks shoppingcarten




//Temp metoder:

app.MapGet("/fillCart", async (StoreService storeService) => await storeService.FillCart());



app.MapRazorPages();
//app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
