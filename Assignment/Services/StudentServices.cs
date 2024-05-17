using Assignment.Models;
using Assignment.Models.Dto;
using Assignment.Services.Interface;
using Assignment_Utility;
using Microsoft.AspNetCore.Hosting.Server;
using System.Runtime.CompilerServices;

namespace Assignment.Services
{
    public class StudentServices : BaseServices, IStudentServices
    {
        public readonly IHttpClientFactory _clientfactory;
        private string villaUrl;
        public StudentServices(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _clientfactory = httpClient;
            villaUrl = configuration.GetValue<string>("ServiceUrls:StudentAPI") ?? "";
        }
        public Task<T> CreateAsync<T>(StudentCreateDto studentCreateDto)
        {
            return SendAysnc<T>(new APIRequest
            {
                ApiType = SD.APIType.POST,
                Data = studentCreateDto,
                url = villaUrl + "StudentApi/Student"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAysnc<T>(new APIRequest
            {
                ApiType = SD.APIType.DELETE,
                url = villaUrl + "StudentApi/Student/" + id
            });
        }

        public Task<T> GetAllAsync<T>(PaginationDTO paginationDTO)
        {
            
            string url = "";
            if(paginationDTO.searchQuery != null)
            {
                url = villaUrl + "StudentApi/Student?searchQuery=" + paginationDTO.searchQuery + "&PageNumber=" + paginationDTO.PageNumber + "&PageSize=" + paginationDTO.PageSize + "&isAcending" + paginationDTO.isAcending;
            }
            else
            {
                url = villaUrl + "StudentApi/Student?PageNumber=" + paginationDTO.PageNumber + "&PageSize=" + paginationDTO.PageSize + "&isAcending" + paginationDTO.isAcending;
            }

            return SendAysnc<T>(new APIRequest
            {
                ApiType = SD.APIType.GET,
                url = url
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

        public Task<T> UpdateAsync<T>(StudentUpdateDto studentUpdateDto)
        {
            return SendAysnc<T>(new APIRequest
            {
                ApiType = SD.APIType.PUT,
                Data = studentUpdateDto,
                url = villaUrl + "StudentApi/Student/" + studentUpdateDto.StudentId
            });
        }
    }
}
