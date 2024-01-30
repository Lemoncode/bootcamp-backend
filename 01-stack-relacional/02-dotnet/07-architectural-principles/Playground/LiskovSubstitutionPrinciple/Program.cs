// See https://aka.ms/new-console-template for more information
using LiskovSubstitutionPrinciple.Entities;
using LiskovSubstitutionPrinciple.Services;

List<Employee> employees = new List<Employee>();
var developer = new Developer { FirstName = "Juanjo", LastName = "Montiel" };
var manager = new Manager { FirstName = "Manolo", LastName = "Dorado" };
var consultant = new Consultant { FirstName = "Armando", LastName = "Bronca Segura" };
employees.Add(developer);
employees.Add(manager);
employees.Add(consultant);
var salaryCalculatorService = new SalaryCalculatorService();

var cuantoMeVoyAGastar = salaryCalculatorService.TotalToPay(employees);
Console.WriteLine($"Este mes vas a tener que pagar a tus empleados un total de {cuantoMeVoyAGastar} euros.");

// Ahora, vamos a hacer lo mismo asignaod proyectos a todos los empleados
var projectService = new ProjectService();

var project = new Project
{
    Name = "Arreglar accesibilidad en CodePaster",
    Description = "Se debe arreglar CodePaster, pues ahora la descripción no se lee con lector de pantalla."
};

projectService.AssignProjectToEmployees(employees, project);
Console.ReadLine();
