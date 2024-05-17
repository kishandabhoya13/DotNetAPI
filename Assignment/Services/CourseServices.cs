using Assignment.Models;
using Assignment.Models.Dto;
using Assignment.Services.Interface;
using Assignment_Utility;

namespace Assignment.Services
{
    public class CourseServices : BaseServices , ICourseServices
    {
        public readonly IHttpClientFactory _clientfactory;
        private string villaUrl;
        public CourseServices(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _clientfactory = httpClient;
            villaUrl = configuration.GetValue<string>("ServiceUrls:StudentAPI") ?? "";
        }
        public Task<T> CreateAsync<T>(CourseCreateDto courseCreateDto)
        {
            return SendAysnc<T>(new APIRequest
            {
                ApiType = SD.APIType.POST,
                Data = courseCreateDto,
                url = villaUrl + "StudentApi/Course"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAysnc<T>(new APIRequest
            {
                ApiType = SD.APIType.DELETE,
                url = villaUrl + "StudentApi/Course/" + id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAysnc<T>(new APIRequest
            {
                ApiType = SD.APIType.GET,
                url = villaUrl + "StudentApi/Course"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAysnc<T>(new APIRequest
            {
                ApiType = SD.APIType.GET,
                url = villaUrl + "StudentApi/Student/" + id
            });
        }

        public Task<T> UpdateAsync<T>(CourseUpdateDto courseUpdateDto)
        {
            return SendAysnc<T>(new APIRequest
            {
                ApiType = SD.APIType.PUT,
                Data = courseUpdateDto,
                url = villaUrl + "StudentApi/Course/" + courseUpdateDto.Id
            });
        }
    }
}
