namespace BLL.Models.GetEntityModels;

public class ItemReadModel
{
    public Guid Id { get; set; }
    public Guid? SenderId { get; set; }
    public Guid? ReceiverId { get; set; }
    public float Weight { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public List<CategoryReadModel> Categories { get; set; } = new List<CategoryReadModel>();
    public Guid? StorageId { get; set; }
    public DateTime Date { get; set; }
    public bool isReceived { get; set; }
}
