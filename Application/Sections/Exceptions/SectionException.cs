using Domain;

namespace Application.Sections.Exceptions;

public abstract class SectionException(/*SectionId id,*/ string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    /*public SectionId SectionId { get; } = id;*/
}

/*public class SectionNotFoundException(SectionId id)
    : SectionException(id, $"Section under id: {id} not found");

public class SectionAlreadyExistsException(SectionId id)
    : SectionException(id, $"Section already exists: {id}");

/*
public class SectionForUserNotFoundException(UserId userId)
    : SectionException(SectionId.Empty(), $"User under id:{userId} not found");#1#

public class SectionUnknownException(SectionId id, Exception innerException)
    : SectionException(id, $"Unknown exception for the section under id: {id}", innerException);*/