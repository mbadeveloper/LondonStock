namespace LondonStock.Resources.Validators.Exceptions
{
    public class ApiClientResponseException : ApiClientException
    {
        public int StatusCode { get; }
        public string ResponseContentString { get; }

        public ApiClientResponseException(string message, int statusCode) : base(message) => StatusCode = statusCode;

        public ApiClientResponseException(string message, int statusCode, string responseContentString)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseContentString = responseContentString;
        }
    }
}
