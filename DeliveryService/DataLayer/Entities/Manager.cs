namespace DataLayer.Entities;

public class Manager : IEntity
{
    public string Name { get; set; }
    public Guid? StorageId { get; set; }
    public Storage? Storage { get; set; }
}
