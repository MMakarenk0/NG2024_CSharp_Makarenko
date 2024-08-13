using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;

namespace BLL.Services.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeReadModel>> GetAllAsync();
    Task<Guid> AddAsync(SaveEmployeeModel model);
    Task<Guid> UpdateAsync(UpdateEmployeeModel model);
    Task DeleteAsync(Guid id);
    Task<EmployeeReadModel> GetByIdAsync(Guid id);
}
