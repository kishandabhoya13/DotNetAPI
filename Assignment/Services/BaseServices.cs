using Assignment.Models;
using Assignment.Services.Interface;
using Assignment_API.Models;
using Assignment_Utility;
using Microsoft.AspNetCore.CookiePolicy;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace Assignment.Services
{
    public class BaseServices : IBaseServices
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient;
        public BaseServices(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            this.httpClient = httpClient;
        }

        public async Task<T> SendAysnc<T>(APIRequest aPIRequest)
        {
            try
            {
                var client =  httpClient.CreateClient("StudentAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(aPIRequest.url);
                if (aPIRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(aPIRequest.Data),
                        Encoding.UTF8, "application/json");
                }
                switch (aPIRequest.ApiType)
                {
                    case SD.APIType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.APIType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.APIType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                await Task.Delay(150);
                HttpResponseMessage apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;
            }
            catch (Exception ex)
            {
                APIResponse dto = new()
                {
                    ErroMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false,
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);

                return APIResponse;
            }
        }
    }
}
