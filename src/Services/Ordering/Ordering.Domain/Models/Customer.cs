

namespace Ordering.Domain.Models
{
    public class Customer : Entity<Guid>
    {
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;

        public static Customer Create(Guid id, string name, string email)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            if (string.IsNullOrWhiteSpace(name))
            {
                //return Error.Failure("Customer.InvalidCustomerName","Name cannot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                //return Error.Failure("Customer.InvalidEmail", "Email cannot be null or whitespace");
            }

            var customer = new Customer
            {
                Id = id,
                Name = name,
                Email = email
            };

            return customer;
        }
    }
}
