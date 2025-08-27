using System.Text.Json.Serialization;

namespace ExchangeRate.Application.DTO.Response
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Errors { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(object data, string message = null)
        {
            Success = true;
            Message = message;
            Data = data;
        }

        public ApiResponse(string message, List<string> errors = null)
        {
            Success = false;
            Message = message;
            Errors = errors;
        }
    }
}
