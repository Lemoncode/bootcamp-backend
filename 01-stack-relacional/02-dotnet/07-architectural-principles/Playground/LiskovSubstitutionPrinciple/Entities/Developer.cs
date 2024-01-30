using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiskovSubstitutionPrinciple.Entities
{
    internal class Developer : Employee
    {
        public override void AssignProject(Project project)
        {
            Console.WriteLine($"OK, se me ha asignado el proyecto {project.Name}.");
        }

        public override decimal CalculateSalary()
        {
            // Calculamos el salario en función, por ejemplo, de las features resueltas multiplicado por el esfuerzo en horas y por el salario base por hora
            return 10000;
        }
    }
}
