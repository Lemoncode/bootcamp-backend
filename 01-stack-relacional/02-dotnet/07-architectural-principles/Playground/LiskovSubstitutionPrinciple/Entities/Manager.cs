using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiskovSubstitutionPrinciple.Entities;

internal class Manager : Employee
{
    public override void AssignProject(Project project)
    {
        Console.WriteLine($"OK, ahora soy el encargado del proyecto {project.Name}.");
    }

    public override decimal CalculateSalary()
    {
        // Podríamos calcular el salario sumando un salario base más bonificaciones por proyectos que se han cerrado en tiempo o antes de la fecha estimada.
        return 13000;
    }
}
