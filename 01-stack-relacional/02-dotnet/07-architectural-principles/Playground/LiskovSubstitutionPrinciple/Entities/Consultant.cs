using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiskovSubstitutionPrinciple.Entities;

internal class Consultant : Employee
{
    public override void AssignProject(Project project)
    {
        throw new InvalidOperationException("Soy consultor, no tengo proyectos ocnretos asignados.");
    }

    public override decimal CalculateSalary()
    {
        // cálculos complejos basados en las horas trabajadas.
        return 8000;
    }
}
