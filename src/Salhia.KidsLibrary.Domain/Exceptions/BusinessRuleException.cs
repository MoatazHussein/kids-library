namespace Salhia.KidsLibrary.Domain.Exceptions
{
    public class BusinessRuleException : BaseException
    {
        public BusinessRuleException(string message, int statusCode = 422, string? errorCode = null)
            : base(message, errorCode ?? "BusinessRuleViolation", statusCode)
        {
        }

        public BusinessRuleException(string message, Exception innerException, int statusCode = 422, string? errorCode = null)
            : base(message, errorCode ?? "BusinessRuleViolation", statusCode)
        {
        }
    }
}
