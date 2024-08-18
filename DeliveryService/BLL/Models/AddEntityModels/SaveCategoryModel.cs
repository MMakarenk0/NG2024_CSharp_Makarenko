namespace BLL.Models.AddEntityModels
{
    public class SaveCategoryModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public List<Guid> ItemIds { get; set; } = new List<Guid>();
    }
}