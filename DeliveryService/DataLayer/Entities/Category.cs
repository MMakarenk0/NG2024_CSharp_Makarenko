namespace DataLayer.Entities;

public class Category : IEntity
{
    public string Description { get; set; }
    public ICollection<ItemCategory> ItemCategories { get; set; } = new List<ItemCategory>();
}