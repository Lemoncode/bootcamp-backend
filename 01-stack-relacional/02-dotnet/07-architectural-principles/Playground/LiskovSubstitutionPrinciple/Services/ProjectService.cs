using LiskovSubstitutionPrinciple.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiskovSubstitutionPrinciple.Services
{
    internal class ProjectService
    {

        public Project CreateProject(string projectName, string projectDescription)
        {
            return new Project { Id = Guid.NewGuid(), Name = projectName, Description = projectDescription };
        }

        public void AssignProjectToEmployees(IEnumerable<Employee> employees, Project project)
        {
            foreach (var employee in employees)
            {
                employee.AssignProject(project);
            }
        }
    }
}
