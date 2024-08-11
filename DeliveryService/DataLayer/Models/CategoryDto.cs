namespace DataLayer.Models;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public List<Guid> ItemIds { get; set; } = new List<Guid>();
}
