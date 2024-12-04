namespace BLL.Models.GetEntityModels
{
    public class ManagerReadModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? StorageId { get; set; }
    }
}