using Assignment.Models.Dto;
using AutoMapper;

namespace Assignment
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<StudentDataModelDto,StudentCreateDto>().ReverseMap();
            CreateMap<StudentDataModelDto, StudentUpdateDto>().ReverseMap();
            CreateMap<CourseModelDTO,CourseCreateDto>().ReverseMap();
            CreateMap<CourseModelDTO,CourseUpdateDto>().ReverseMap();
        }
    }
}
