using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiskovSubstitutionPrinciple.Entities;

internal abstract class Employee
{

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public abstract decimal CalculateSalary();
    public abstract void AssignProject(Project project);
}
