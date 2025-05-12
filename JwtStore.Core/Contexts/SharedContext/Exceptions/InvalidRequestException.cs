namespace JwtStore.Core.Contexts.SharedContext.Exceptions
{
    public class InvalidRequestException : Exception
    {
        private const string DefaultErrorMessage = "Não foi possível validar sua requisição";
        private const int DefaultStatusCode = 500;
        public int StatusCode { get; }
        public InvalidRequestException(string? message = null, int statusCode = DefaultStatusCode) 
            : base(message ?? DefaultErrorMessage) => StatusCode = statusCode;
    }
}