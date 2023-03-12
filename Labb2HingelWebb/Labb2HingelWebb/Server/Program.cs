using Labb2HingelWebb.Server.Data;
using Labb2HingelWebb.Server.Extensions;
using Labb2HingelWebb.Server.Models;
using Labb2HingelWebb.Server.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
	.AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
	.AddGoogle(googleOptions =>
	{
		googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
		googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
	})
	.AddIdentityServerJwt();

builder.Services.AddAuthorizationBuilder()
	.AddPolicy("AdminAccess", policy => policy.RequireRole("admin"));



builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//Egna tillagda delar:

builder.Services.AddScoped<IProductRepository<StoreProduct>, ProductRepository>();
builder.Services.AddScoped<IOrderRepository<Order>, OrderRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<CustomerService>();

builder.Services.AddScoped<RoleManager<IdentityRole>>();

builder.Services.AddScoped<RoleService>();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");


app.MapStoreEndPoints();

app.MapCustomerEndPoints();

app.MapOrderEndPoints();

app.MapGet("/hello", async () =>
{
	using (var scope = app.Services.CreateScope())
	{
		var rolemgt = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

		var usrMngr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
		//await rolemgt.CreateAsync(new IdentityRole() {Name = "admin"});

		//var nUser = new ApplicationUser() { UserName = "test", Email = "test@test.se" };

		var role = rolemgt.Roles.FirstOrDefault(r => r.Name== "admin");

		var nUser = await usrMngr.FindByEmailAsync("henrik.ingelsten@gmail.com");

			//await usrMngr.CreateAsync(nUser, "Abcd123!");
			//if (nUser is not null)
				//await usrMngr.AddToRoleAsync(nUser, role.Name);

	}
});


app.MapGet( "/test", () => "hej"); //.RequireAuthorization("admin_greetings");



app.Run();
