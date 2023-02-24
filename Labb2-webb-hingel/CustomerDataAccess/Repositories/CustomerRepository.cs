using CustomerDataAccess.Context;
using CustomerDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CustomerDataAccess.Repositories;

public class CustomerRepository : ICustomerRepository
{
	private CustomerContext _customersContext;

	public CustomerRepository(CustomerContext customerContext)
	{
		_customersContext = customerContext;
	}

	public async Task<IEnumerable<Customer>> GetAll()
	{
		throw new NotImplementedException();
	}

	public async Task<Customer> GetById(Guid id)
	{
		var customer = await _customersContext.Customers.FirstOrDefaultAsync(c => c.Id.Equals(id));

		return customer;
	}

	public async Task<Customer> GetByEmail(string email)
	{
		var customer = await _customersContext.Customers.FirstOrDefaultAsync(c => c.Email.Equals(email));

		//TODO: Kolla not null

		return customer;
	}

	public async Task<bool> PostNewCustomer(Customer newCustomer)
	{
		if (await CustomerExists(newCustomer))
		{
			//TODO: Returnera att det gått NOK.
			return false;
		}

		await _customersContext.AddAsync(newCustomer);
		await _customersContext.SaveChangesAsync();

		return true;
		//TODO: Retuerna att det gått ok.
	}

	private async Task<bool> CustomerExists(Customer newCustomer)
	{
		return await _customersContext.Customers.AnyAsync(c => c.Email.ToLower() == newCustomer.Email.ToLower());
	}

	public async Task UpdateCustomer(string email, Customer updatedCustomer)
	{
		throw new NotImplementedException();
	}

	public async Task DeleteCustomer(Guid id)
	{
		throw new NotImplementedException();
	}
}