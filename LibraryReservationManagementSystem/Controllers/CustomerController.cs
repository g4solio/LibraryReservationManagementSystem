using LibraryReservationManagementSystem.Models;
using LibraryReservationManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryReservationManagementSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController(RepositoryFactory repositoryFactory, ILogger<BookController> logger) : ControllerBase
{
    private readonly ILogger<BookController> _logger = logger;

    [HttpGet(Name = "GetCustomers")]
    public IActionResult Get()
    {
        using var repository = repositoryFactory.GetRepository<Customer>();
        var customers = repository.GetAll();
        return customers.IsSuccess
            ? Ok(customers.Data)
            : NotFound(customers.Message);
    }

    [HttpGet("{id}", Name = "GetCustomer")]
    public IActionResult Get(int id)
    {
        using var repository = repositoryFactory.GetRepository<Customer>();
        var customer = repository.GetById(id);
        return customer.IsSuccess
            ? Ok(customer.Data)
            : NotFound(customer.Message);
    }

    #region Context

    public class CreateCustomerRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }

        public Customer ToCustomer()
        {
            return new Customer
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                RegistrationDate = RegistrationDate
            };
        }
    }

    #endregion

    [HttpPost(Name = "CreateCustomer")]
    public IActionResult Post(CreateCustomerRequest customer)
    {
        using var repository = repositoryFactory.GetRepository<Customer>();
        var result = repository.Add(customer.ToCustomer());
        return result.IsSuccess
            ? Ok(result.Data)
            : BadRequest(result.Message);
    }

    #region Context

    public class UpdateCustomerRequest
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Customer ToCustomer()
        {
            return new Customer
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                RegistrationDate = RegistrationDate
            };
        }
    }

    #endregion

    [HttpPut(Name = "UpdateCustomer")]
    public IActionResult Put(int id, UpdateCustomerRequest customer)
    {
        using var repository = repositoryFactory.GetRepository<Customer>();
        var result = repository.Update(customer.ToCustomer());
        return result.IsSuccess
            ? Ok(result.Data)
            : BadRequest(result.Message);
    }

    [HttpDelete("{id}", Name = "DeleteCustomer")]
    public IActionResult Delete(int id)
    {
        using var repository = repositoryFactory.GetRepository<Customer>();
        var customerToDelete = repository.GetById(id);

        if (!customerToDelete.IsSuccess || customerToDelete.Data == null)
            return NotFound(customerToDelete.Message);
        
        var result = repository.Delete(customerToDelete.Data);
        return result.IsSuccess
            ? Ok(result.Data)
            : BadRequest(result.Message);
    }
}