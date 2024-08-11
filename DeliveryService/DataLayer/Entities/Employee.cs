namespace DataLayer.Entities;

public class Employee : IEntity
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public decimal Salary { get; set; }
    public string Position { get; set; }
    public Guid? StorageId { get; set; }
    public Storage? Storage { get; set; }
}
