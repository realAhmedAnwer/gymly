namespace Gymly.Application.Common.Exceptions;

public class GymlyValidationException(string message) : InvalidOperationException(message)
{
}