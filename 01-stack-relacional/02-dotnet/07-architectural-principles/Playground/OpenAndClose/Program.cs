// See https://aka.ms/new-console-template for more information
using OpenAndClose;
using OpenAndClose.Entities;

var radius = 5d;
var circle = new Circle { Radius = radius };
var areaCalculator = new AreaCalculator();
Console.WriteLine($"El área del círculo es: {areaCalculator.TotalArea(new[] { circle })}.");