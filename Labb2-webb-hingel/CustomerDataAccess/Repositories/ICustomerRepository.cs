using CustomerDataAccess.Models;

namespace CustomerDataAccess.Repositories;

public interface ICustomerRepository
{
	Task<IEnumerable<Customer>> GetAll();
	Task<Customer> GetById(Guid id);
	Task<Customer> GetByEmail(string email);
	Task PostNewCustomer(Customer newCustomer);
	Task UpdateCustomer (string email, Customer updatedCustomer);
	Task DeleteCustomer (Guid id);
}