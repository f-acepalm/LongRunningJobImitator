namespace LongRunningJobImitator.Services.Exceptions;

public class ValidationException : ApplicationException
{
    public ValidationException()
    {
    }

    public ValidationException(string message) : base(message)
    {
    }
}
