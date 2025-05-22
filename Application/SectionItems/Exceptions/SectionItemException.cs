using Domain;

namespace Application.SectionItems.Exceptions;

public abstract class SectionItemException(string id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public string SectionItemId { get; } = id;
}

public class SectionItemNotFoundException(string id)
    : SectionItemException(id, $"Section item under id: {id} not found");

public class SectionItemAlreadyExistsException(string title)
    : SectionItemException("-", $"Опис секції з назвою \"{title}\" вже існує.");

public class SectionItemUnknownException(string id, Exception innerException)
    : SectionItemException(id, $"Unknown exception for the section item under id: {id}", innerException);