using CustomerDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerDataAccess.Context;

public class CustomerContext : DbContext
{
	public DbSet<Customer> Customers { get; set; }

	public CustomerContext(DbContextOptions options) : base(options)
	{

	}
}