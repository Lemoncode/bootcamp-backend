using LiskovSubstitutionPrinciple.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiskovSubstitutionPrinciple.Services
{
    internal class SalaryCalculatorService
    {

        public decimal TotalToPay(IEnumerable<Employee> employees)
        {
            return employees.Sum(e => e.CalculateSalary());
        }
    }
}
