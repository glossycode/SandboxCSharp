using AutoMapper;
using System;
using Unity;

namespace SandboxCSharp.Mapper
{
    class MapperFactory : IFactory
    {
        public void DoRegister(IUnityContainer Container)
        {
            // using the nuget AutoMapper
        }

        public void Run(IUnityContainer Container)
        {            
            //Step1: Initialize the mapper
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.FullName, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.Dept, act => act.MapFrom(src => src.Department));
                cfg.CreateMap<EmployeeDTO, Employee>()
                 .ForMember(dest => dest.Name, act => act.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Department, act => act.MapFrom(src => src.Dept));
            });
            // only during development, validate your mappings; remove it before release
#if DEBUG
            configuration.AssertConfigurationIsValid();
#endif
            // use DI (http://docs.automapper.org/en/latest/Dependency-injection.html) or create the mapper yourself
            var mapper = configuration.CreateMapper();


            //Step2: Create the source object
            Employee emp = new Employee();
            emp.Name = "James";
            emp.Salary = 20000;
            emp.Address = "London";
            emp.Department = "IT";
            //Step3: use the mapper to map the source and destination object
            var empDTO = mapper.Map<Employee, EmployeeDTO>(emp);
            //OR
            //var empDTO = Mapper.Map<EmployeeDTO>(emp);
            Console.WriteLine("Name:" + empDTO.FullName + ", Salary:" + empDTO.Salary + ", Address:" + empDTO.Address + ", Department:" + empDTO.Dept);
        }
    }


    public class Employee
    {
        public string Name { get; set; }
        public int Salary { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
    }
    public class EmployeeDTO
    {
        public string FullName { get; set; }
        public int Salary { get; set; }
        public string Address { get; set; }
        public string Dept { get; set; }
    }
}
