using AutoMapper;
using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;
using DataLayer.Entities;

namespace BLL.Mapping;
public class AutomapperBLLProfile : Profile
{
    public AutomapperBLLProfile()
    {
        CreateMap<Item, ItemReadModel>()
            .ReverseMap();

        CreateMap<Item, SaveItemModel>()
            .ForMember(
                dest => dest.CategoryIds,
                opt => opt.MapFrom(src => src.ItemCategories.Select(ic => ic.CategoryId).ToList()))
            .ReverseMap();

        CreateMap<Item, UpdateItemModel>()
            .ForMember(
                dest => dest.CategoryIds,
                opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Category, CategoryReadModel>()
            .ReverseMap();

        CreateMap<Category, SaveCategoryModel>()
            .ForMember(
                dest => dest.ItemIds,
                opt => opt.MapFrom(src => src.ItemCategories.Select(ic => ic.ItemId).ToList()))
            .ReverseMap();

        CreateMap<Category, UpdateCategoryModel>()
            .ForMember(
                dest => dest.ItemIds,
                opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Storage, StorageReadModel>()
            .ReverseMap();

        CreateMap<Storage, SaveStorageModel>()
            .ForMember(dest => dest.DirectorId, opt => opt.Ignore())
            .ForMember(
                dest => dest.EmployeeIds,
                opt => opt.MapFrom(src => src.Employees.Select(ic => ic.Id).ToList()))
            .ForMember(
                dest => dest.ItemIds,
                opt => opt.MapFrom(src => src.Items.Select(ic => ic.Id).ToList()))
            .ReverseMap();

        CreateMap<Storage, UpdateStorageModel>()
            .ForMember(dest => dest.DirectorId, opt => opt.Ignore())
            .ForMember(dest => dest.EmployeeIds, opt => opt.Ignore())
            .ForMember(dest => dest.ItemIds, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Employee, EmployeeReadModel>() 
            .ReverseMap();

        CreateMap<Employee, SaveEmployeeModel>()
            .ReverseMap();

        CreateMap<Employee, UpdateEmployeeModel>()
            .ForMember(dest => dest.StorageId, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Manager, ManagerReadModel>()
            .ReverseMap();

        CreateMap<Manager, SaveManagerModel>()
            .ForMember(dest => dest.StorageId, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Manager, UpdateManagerModel>()
            .ForMember(dest => dest.StorageId, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Client, ClientReadModel>()
            .ReverseMap();

        CreateMap<Client, SaveClientModel>()
            .ReverseMap();

        CreateMap<Client, UpdateClientModel>()
            .ReverseMap();
    }
}
