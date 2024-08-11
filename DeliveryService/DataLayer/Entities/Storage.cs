namespace DataLayer.Entities;

public class Storage : IEntity
{
    public string Address { get; set; }
    public int Number { get; set; }
    public Guid? DirectorId { get; set; }
    public Manager? Director { get; set; }
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<Item> Items { get; set; } = new List<Item>();
}
