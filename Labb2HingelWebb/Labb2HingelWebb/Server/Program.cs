using System.Data;
using Labb2HingelWebb.Client;
using Labb2HingelWebb.Server.Data;
using Labb2HingelWebb.Server.Models;
using Labb2HingelWebb.Server.Services;
using Labb2HingelWebb.Shared.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using StoreDataAccess.Models;
using StoreDataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
	.AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
	.AddIdentityServerJwt();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//Egna tillagda delar:

builder.Services.AddScoped<IStoreRepository<StoreProduct>, ProductRepository>();
builder.Services.AddScoped<IStoreRepository<Order>, OrderRepository>();
builder.Services.AddScoped<StoreService>();


builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<UserManager<ApplicationUser>>();

//Får inte detta att funka, men får fixa det sen.
//builder.Services.AddScoped<RoleManager<IdentityRole>>();
//builder.Services.AddScoped<RoleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
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





app.UseIdentityServer();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

//Customer APIs
app.MapGet("/getUser", async (CustomerService customerService, string email) =>
{
	await customerService.FindUser(email);
});

app.MapGet("/findUserByName/{name}", async (CustomerService customerService, string name) =>
{
	return await customerService.FindUserByName(name);
});

app.MapGet("/findCustomers", async (CustomerService customerService) =>
{
	return await customerService.FindAllUsers();
});

app.MapPost("/updateUser", async (CustomerService customerService, CustomerDto updatedCustomerDto) =>
{
	return await customerService.UpdateUser(updatedCustomerDto);
});

//StoreItem APIs Typ allt är admin grejer.
app.MapPost("/addStoreProduct",
	async (StoreService storeService, StoreProductDto newDtoProduct) =>
	{
		await storeService.AddNewProduct(newDtoProduct);
	});

app.MapGet("/getAllProducts", async (StoreService storeService) =>
{
	return await storeService.GetAllProducts();
});

app.MapDelete("/deleteProduct/{productName}", async (StoreService storeService, string productName) =>
{
	await storeService.DeleteProduct(productName);
});

app.MapGet("/getProductByName", async (StoreService storeService, string searchName) => await storeService.GetByName(searchName));


app.MapGet("/getProductByNumber", async (StoreService storeService, string id) => await storeService.GetById(id));
//app.MapPut("/getDiscontinueProduct", async (StoreService storeService, string searchName) => await storeService.DiscontinueItem(searchName));



//Orders APIs, de mesta är för admin, måste kolla att vem som helst inte kan köra dessa:

app.MapPost("/placeOrder", async (StoreService storeService, OrderDto newOrder) =>
{
	await storeService.PlaceOrder(newOrder);
});

app.MapGet("/getCustomerOrders/{email}", async (StoreService storeService, string email) =>
{
	return await storeService.GetOrders(email);
});

//Få tillbaks shoppingcarten


app.Run();
