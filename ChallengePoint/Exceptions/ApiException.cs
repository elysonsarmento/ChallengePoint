namespace ChallengePoint.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }

        public ApiException() : base("An error occurred in the application.")
        {
            StatusCode = 500;
        }

        public ApiException(string message) : base(message)
        {
            StatusCode = 500;
        }

        public ApiException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public ApiException(string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = 500;
        }

        public ApiException(string message, int statusCode, Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }

    
}
