using AutoMapper;
using EmployeeManagementAPI.Models.Data;
using EmployeeManagementAPI.Models.DTO.Attendance;
using EmployeeManagementAPI.Models.DTO.Auth;
using EmployeeManagementAPI.Models.DTO.Department;
using EmployeeManagementAPI.Models.DTO.Employee;

namespace EmployeeManagementAPI.Configurations
{
    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreCommonCreateFields<TSource, TDestination>(
    this IMappingExpression<TSource, TDestination> map)
        {
            var destinationType = typeof(TDestination);

            map.ForMember("Id", opt => opt.Ignore())
               .ForMember("CreatedAt", opt => opt.Ignore());

            if (destinationType.GetProperty("UpdatedAt") != null)
            {
                map.ForMember("UpdatedAt", opt => opt.Ignore());
            }

            map.ForAllMembers(opt =>
            {
                opt.Condition((src, dest, srcMember) => srcMember != null);
            });

            return map;
        }

    }

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Employee mappings
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.DepartmentName,
                           opt => opt.MapFrom(src => src.Department != null
                                                         ? src.Department.Name
                                                         : string.Empty))
                .ForMember(dest => dest.IsActive,
                           opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.HireDate,
                           opt => opt.MapFrom(src => src.HireDate));

            CreateMap<CreateEmployeeDto, Employee>()
                .IgnoreCommonCreateFields()
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Attendances, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<UpdateEmployeeDto, Employee>()
                .IgnoreCommonCreateFields()
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Attendances, opt => opt.Ignore());

            CreateMap<CreateDepartmentDto, Department>().IgnoreCommonCreateFields()
    .ForMember(dest => dest.Employees, opt => opt.Ignore());

            CreateMap<UpdateDepartmentDto, Department>().IgnoreCommonCreateFields()
                .ForMember(dest => dest.Employees, opt => opt.Ignore());

            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.Employees != null ? src.Employees.Count(e => e.IsActive) : 0))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description)); 


            CreateMap<Attendance, AttendanceDto>()
                .ForMember(
                    dest => dest.EmployeeName,
                    opt => opt.MapFrom(
                        src =>
                            src.Employee != null
                                ? $"{src.Employee.FirstName} {src.Employee.LastName}"
                                : string.Empty))
                .ForMember(dest => dest.WorkingHours,
                           opt => opt.MapFrom(
                               src => src.WorkingHours));  // include WorkingHours

            CreateMap<CreateAttendanceDto, Attendance>()
                .IgnoreCommonCreateFields()
                .ForMember(dest => dest.Id,           opt => opt.Ignore())
                .ForMember(dest => dest.Employee,     opt => opt.Ignore())
                .ForMember(dest => dest.WorkingHours, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt,    opt => opt.Ignore())
                .ForMember(dest => dest.CheckInTime,  opt => opt.MapFrom(src => ParseTime(src.CheckInTime)))
                .ForMember(dest => dest.CheckOutTime, opt => opt.MapFrom(src => ParseTime(src.CheckOutTime)));

            CreateMap<UpdateAttendanceDto, Attendance>()
                .IgnoreCommonCreateFields()
                .ForMember(dest => dest.Employee,     opt => opt.Ignore())
                .ForMember(dest => dest.CheckInTime,  opt => opt.MapFrom(src => ParseTime(src.CheckInTime)))
                .ForMember(dest => dest.CheckOutTime, opt => opt.MapFrom(src => ParseTime(src.CheckOutTime)));

            // User/Auth mappings
            CreateMap<RegisterDto, User>()
                .IgnoreCommonCreateFields()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.RequestedRole, opt => opt.MapFrom(src => src.RequestedRole))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<User, AuthResponseDto>()
                .ForMember(dest => dest.Token, opt => opt.Ignore())
                .ForMember(dest => dest.Expires, opt => opt.Ignore());
        }

        private static TimeSpan? ParseTime(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return TimeSpan.TryParse(value, out var ts) ? ts : (TimeSpan?)null;
        }
    }
}
