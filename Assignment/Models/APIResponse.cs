using System.Net;

namespace Assignment_API.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess { get; set; } = true;
        
        public List<string> ErroMessages { get; set; }

        public object result { get; set; }
    }
}
