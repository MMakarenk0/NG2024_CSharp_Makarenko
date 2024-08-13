namespace BLL.Models.GetEntityModels
{
    public class CategoryReadModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public List<ItemReadModel> Items { get; set; } = new List<ItemReadModel>();
    }
}