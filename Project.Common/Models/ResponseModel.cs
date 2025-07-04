namespace Project.Common.Models
{
    public class ResponseModel<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public bool IsError { get; set; }
    }
}
