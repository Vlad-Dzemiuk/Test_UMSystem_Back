using Domain;

namespace Application.Users.Exceptions;

public class UserException(string id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public string UserId { get; } = id;
    
}

public class UserNotFoundException(string id)
    : UserException(id, $"User under id: {id} not found");

public class UserAlreadyExistsException(string FirstName)
    : UserException("-", $"Користувач з ім'ям \"{FirstName}\" вже існує.");

public class UserUnknownException(string id, Exception innerException)
    : UserException(id, $"Unknown exception for the user under id: {id}", innerException);