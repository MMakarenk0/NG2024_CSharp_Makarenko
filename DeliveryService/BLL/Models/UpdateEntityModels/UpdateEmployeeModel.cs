namespace BLL.Models.UpdateEntityModels
{
    public class UpdateEmployeeModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public decimal Salary { get; set; }
        public string Position { get; set; }
        public Guid? StorageId { get; set; }
    }
}