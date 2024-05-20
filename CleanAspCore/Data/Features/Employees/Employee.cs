﻿using CleanAspCore.Data.Features.Departments;
using CleanAspCore.Data.Features.Jobs;

namespace CleanAspCore.Data.Features.Employees;

public class Employee : IEntity
{
    public required Guid Id { get; init; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required EmailAddress Email { get; set; }
    public required string Gender { get; set; }
    public virtual Department? Department { get; init; }
    public required Guid DepartmentId { get; set; }
    public virtual Job? Job { get; init; }
    public required Guid JobId { get; set; }
}