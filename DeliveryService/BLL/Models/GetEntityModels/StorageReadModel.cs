namespace BLL.Models.GetEntityModels
{
    public class StorageReadModel
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public int Number { get; set; }
        public Guid? DirectorId { get; set; }
        public List<EmployeeReadModel> Employees { get; set; } = new List<EmployeeReadModel>();
        public List<ItemReadModel> Items { get; set; } = new List<ItemReadModel>();
    }
}