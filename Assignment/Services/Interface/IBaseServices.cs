using Assignment.Models;
using Assignment_API.Models;

namespace Assignment.Services.Interface
{
    public interface IBaseServices
    {
        APIResponse responseModel { get; set; }

        Task<T> SendAysnc<T>(APIRequest aPIRequest);
    }
}
