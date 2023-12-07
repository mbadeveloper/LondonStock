namespace LondonStock.Resources.Validators.Exceptions
{
    public class ApiClientException : Exception
    {
        public ApiClientException(string message, Exception? innerException) : base(message, innerException)
        {
        }

        public ApiClientException(string message) : this(message, null)
        {
        }
    }
}
