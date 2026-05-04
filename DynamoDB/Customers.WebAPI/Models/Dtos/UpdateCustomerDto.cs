namespace Customers.WebAPI.Models.Dtos;

public sealed record UpdateCustomerDto(Guid Id, string Name, string Email);