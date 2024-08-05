namespace DataLayer.Models;

public class StorageDto
{
    public Guid Id { get; set; }
    public string Address { get; set; }
    public int Number { get; set; }
    public Guid? DirectorId { get; set; }
    public List<Guid>? EmployeesIds { get; set; } = new List<Guid>();
    public List<Guid>? ItemsIds { get; set; } = new List<Guid>();
}
