using Domain.Section_Items;
using Domain.Sections;

namespace Application.SectionItems.Exceptions;

public abstract class SectionItemException(SectionItemId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public SectionItemId SectionItemId { get; } = id;
}

public class SectionItemNotFoundException(SectionItemId id)
    : SectionItemException(id, $"Section item under id: {id} not found");

public class SectionItemAlreadyExistsException(SectionItemId id)
    : SectionItemException(id, $"Section item already exists: {id}");

public class SectionItemForSectionNotFoundException(SectionId sectionId)
    : SectionItemException(SectionItemId.Empty(), $"Section under id:{sectionId} not found");

public class SectionItemUnknownException(SectionItemId id, Exception innerException)
    : SectionItemException(id, $"Unknown exception for the section item under id: {id}", innerException);