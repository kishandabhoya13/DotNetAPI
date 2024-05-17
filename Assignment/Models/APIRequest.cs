using static Assignment_Utility.SD;

namespace Assignment.Models
{
    public class APIRequest
    {
        public APIType ApiType { get; set; } = APIType.GET;

        public string url { get; set; }

        public object Data { get; set; }
    }
}
