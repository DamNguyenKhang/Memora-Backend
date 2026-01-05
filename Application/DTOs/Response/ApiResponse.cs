using System.Text.Json.Serialization;

namespace Application.DTOs.Response
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Result { get; set; }
    }
}
