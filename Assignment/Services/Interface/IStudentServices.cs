using Assignment.Models.Dto;

namespace Assignment.Services.Interface
{
    public interface IStudentServices
    {
        Task<T> GetAllAsync<T>(PaginationDTO paginationDTO);

        Task<T> GetAsync<T>(int id);

        Task<T> CreateAsync<T>(StudentCreateDto studentCreateDto);

        Task<T> UpdateAsync<T>(StudentUpdateDto studentUpdateDto);

        Task<T> DeleteAsync<T>(int id);
    }
}
