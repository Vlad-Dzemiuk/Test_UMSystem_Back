using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Sections.Exceptions;
using MediatR;

namespace Application.Sections.Commands;

public class DeleteSectionCommand : IRequest<Unit>
{
    public string SectionId { get; }

    public DeleteSectionCommand(string sectionId)
    {
        SectionId = sectionId;
    }
}

public class DeleteSectionCommandHandler : IRequestHandler<DeleteSectionCommand, Unit>
{
    private readonly ISectionRepository _sectionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserQueries _userQueries;

    public DeleteSectionCommandHandler(ISectionRepository sectionRepository, IUserRepository userRepository, IUserQueries userQueries)
    {
        _sectionRepository = sectionRepository;
        _userRepository = userRepository;
        _userQueries = userQueries;
    }

    public async Task<Unit> Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
    {
        var existingSection = await _sectionRepository.GetById(request.SectionId);
        if (existingSection == null)
        {
            throw new SectionNotFoundException(request.SectionId);
        }

        var user = await _userQueries.GetById(existingSection.UserId);
        if (user != null)
        {
            user.Sections.RemoveAll(s => s.SectionId == existingSection.SectionId);
            await _userRepository.Update(user.UserId, user);
        }
        
        await _sectionRepository.Delete(request.SectionId);
        return Unit.Value;
    }
}