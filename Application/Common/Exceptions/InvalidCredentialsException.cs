namespace Application.Common.Exceptions;

public class InvalidCredentialsException : ApplicationException
{
    public InvalidCredentialsException(string message) : base(message) { }
}
