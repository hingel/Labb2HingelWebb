using CustomerDataAccess.Context;
using CustomerDataAccess.Models;

namespace CustomerDataAccess.Repositories;

public class CustomerRepository : ICustomerRepository
{
	private CustomerContext _customers;

	public CustomerRepository(CustomerContext customerContext)
	{
		_customers = customerContext;
	}

	public async Task<IEnumerable<Customer>> GetAll()
	{
		throw new NotImplementedException();
	}

	public async Task<Customer> GetById(Guid id)
	{
		throw new NotImplementedException();
	}

	public async Task<Customer> GetByEmail(string email)
	{
		throw new NotImplementedException();
	}

	public async Task PostNewCustomer(Customer newCustomer)
	{
		throw new NotImplementedException();
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