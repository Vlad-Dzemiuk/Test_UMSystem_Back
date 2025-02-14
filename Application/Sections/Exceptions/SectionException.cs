using Domain;

namespace Application.Sections.Exceptions;

public abstract class SectionException(string id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public string SectionId { get; } = id;
}

public class SectionNotFoundException(string id)
    : SectionException(id, $"Section under id: {id} not found");

public class SectionAlreadyExistsException(string id)
    : SectionException(id, $"Section already exists: {id}");

/*public class SectionForUserNotFoundException(string userId)
    : SectionException(Section.Empty(), $"User under id:{userId} not found");*/

public class SectionUnknownException(string id, Exception innerException)
    : SectionException(id, $"Unknown exception for the section under id: {id}", innerException);