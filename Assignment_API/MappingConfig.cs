using Assignment_API.DataModals;
using Assignment_API.Models.Dto;
using AutoMapper;

namespace Assignment_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<Student, StudentDataModelDto>().ReverseMap();
            CreateMap<Student,StudentCreateDto>().ReverseMap();
            CreateMap<Student, StudentUpdateDto>().ReverseMap();
            CreateMap<Course,CourseModelDTO>().ReverseMap();
            CreateMap<Course,CourseCreateDto>().ReverseMap();
            CreateMap<Course,CourseUpdateDto>().ReverseMap();
        }
    }
}
