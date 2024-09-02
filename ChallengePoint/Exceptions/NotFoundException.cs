namespace ChallengePoint.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException() : base("The requested resource was not found.", 404)
        {
        }

        public NotFoundException(string message) : base(message, 404)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, 404, innerException)
        {
        }
    }
}
