using Domain;

namespace Application.Sections.Exceptions;

public abstract class SectionException(string id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public string SectionId { get; } = id;
}

public class SectionNotFoundException(string id)
    : SectionException(id, $"Section under id: {id} not found");

public class SectionAlreadyExistsException(string name)
    : SectionException("-", $"Секція з назвою \"{name}\" вже існує.");

public class SectionUnknownException(string id, Exception innerException)
    : SectionException(id, $"Unknown exception for the section under id: {id}", innerException);