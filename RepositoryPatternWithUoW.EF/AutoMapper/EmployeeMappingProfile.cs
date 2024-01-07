using AutoMapper;
using RepositoryPatternWithUoW.Core.Models.Domain;
using RepositoryPatternWithUoW.Core.Models.Dto.Employee;
using RepositoryPatternWithUoW.Core.Models.Helper;
using RepositoryPatternWithUoW.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUoW.Core.Models.AutoMapper
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<Employees, EmployeeGetModel>()
           .ForMember(dest => dest.AcademicLevel, opt => opt.MapFrom(src => src.AcademicLevel))
           .ForMember(dest => dest.createdBy, opt => opt.MapFrom(src => src.CreatedUser != null ? $"{src.CreatedUser.FirstName} {src.CreatedUser.LastName}" : ""))
           .ForMember(dest => dest.modifiedBy, opt => opt.MapFrom(src => src.ModifiedUser != null ? $"{src.ModifiedUser.FirstName} {src.ModifiedUser.LastName}" : ""))
           .ForMember(dest => dest.AcademicLevelValue, opt => opt.MapFrom(src => (int)src.AcademicLevel))
           .ForMember(dest => dest.image, opt => opt.MapFrom( src => src.image != null ? MediaControl.GetPath(Enum.FilePath.EmployeeImage)+src.image : ""));

            CreateMap<Employees, EmployeeAddEditModel>()
           .ForMember(dest => dest.AcademicLevel, opt => opt.MapFrom(src => src.AcademicLevel))
           .ForMember(dest => dest.createdBy, opt => opt.MapFrom(src => src.CreatedUser != null ? $"{src.CreatedUser.FirstName} {src.CreatedUser.LastName}" : ""))
           .ForMember(dest => dest.image, opt => opt.MapFrom(src => src.image != null ? MediaControl.GetPath(Enum.FilePath.EmployeeImage) + src.image : ""))
           .ForMember(dest => dest.modifiedBy, opt => opt.MapFrom(src => src.ModifiedUser != null ? $"{src.ModifiedUser.FirstName} {src.ModifiedUser.LastName}" : ""))
           .ForMember(dest => dest.image, opt => opt.MapFrom(src => src.image != null ? MediaControl.GetPath(Enum.FilePath.EmployeeImage) + src.image : ""));
        }
    }
}
