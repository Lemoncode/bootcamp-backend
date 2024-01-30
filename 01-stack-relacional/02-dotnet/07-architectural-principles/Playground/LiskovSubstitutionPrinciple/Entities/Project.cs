namespace LiskovSubstitutionPrinciple.Entities;

internal class Project
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

}