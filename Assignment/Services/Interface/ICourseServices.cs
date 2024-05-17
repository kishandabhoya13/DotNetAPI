using Assignment.Models.Dto;

namespace Assignment.Services.Interface
{
    public interface ICourseServices
    {
        Task<T> GetAllAsync<T>();

        Task<T> GetAsync<T>(int id);

        Task<T> CreateAsync<T>(CourseCreateDto courseCreateDto);

        Task<T> UpdateAsync<T>(CourseUpdateDto courseUpdateDto);

        Task<T> DeleteAsync<T>(int id);
    }
}
