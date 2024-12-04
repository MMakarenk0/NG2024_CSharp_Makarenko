using BLL.Models.GetEntityModels;

namespace BLL.Models.UpdateEntityModels
{
    public class UpdateStorageModel
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public int Number { get; set; }
        public Guid? DirectorId { get; set; }
        public List<Guid> EmployeeIds { get; set; } = new List<Guid>();
        public List<Guid> ItemIds { get; set; } = new List<Guid>();
    }
}