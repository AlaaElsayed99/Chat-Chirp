namespace API.Errors
{
    public class ApiException
    {
        public ApiException(int statusCode, string message, string details)
        {
            this.statusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int statusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}
