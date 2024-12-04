namespace BLL.Models.UpdateEntityModels
{
    public class UpdateCategoryModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public List<Guid> ItemIds { get; set; } = new List<Guid>();
    }
}