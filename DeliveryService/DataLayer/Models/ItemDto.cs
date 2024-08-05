namespace DataLayer.Models;

public class ItemDto
{
    public Guid Id { get; set; }
    public Guid? SenderId { get; set; }
    public Guid? ReceiverId { get; set; }
    public float Weight { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public List<Guid> CategoryIds { get; set; } = new List<Guid>();
    public Guid? StorageId { get; set; }
}
