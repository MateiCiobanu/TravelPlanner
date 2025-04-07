using System;

namespace TravelPlanner.Domain.Entities;

public class User
{
    public int Id { get; set;}
    public string FirstName { get; private set;} = string.Empty;
    public string LastName { get; private set;} = string.Empty ;
    public string Email { get; set;} = string.Empty;
}
