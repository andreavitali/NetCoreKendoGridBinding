using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreKendoAngularGridBinding
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dto => dto.FullName, m => m.MapFrom(e => e.FirstName + " " + e.LastName))
                .ForMember(dto => dto.Department, m => m.MapFrom(e => e.Department.Description));
        }
    }
}
