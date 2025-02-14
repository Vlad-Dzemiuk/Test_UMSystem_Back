using API.Dtos;
using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Sections.Exceptions;
using Application.Users.Commands;
using Application.Users.Exceptions;
using Domain;
using MediatR;

namespace Application.Sections.Commands;

public class UpdateSectionCommand : IRequest<SectionDto>
{
    public string SectionId { get; }
    public string Name { get; }
    public string UserId { get; }
    
    public UpdateSectionCommand(string sectionId, UpdateSectionDto dto)
    {
        SectionId = sectionId;
        Name = dto.Name;
        UserId = dto.UserId;
    }
}

public class UpdateSectionCommandHandler : IRequestHandler<UpdateSectionCommand, SectionDto>
{
    private readonly ISectionRepository _sectionRepository;
    private readonly ISectionQueries _sectionQueries;
    private readonly IUserRepository _userRepository;
    private readonly IUserQueries _userQueries;

    public UpdateSectionCommandHandler(
        ISectionQueries sectionQueries, 
        ISectionRepository sectionRepository, 
        IUserQueries userQueries, 
        IUserRepository userRepository)
    {
        _sectionQueries = sectionQueries;
        _sectionRepository = sectionRepository;
        _userQueries = userQueries;
        _userRepository = userRepository;
    }

    public async Task<SectionDto> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
    {
        var existingSection = await _sectionQueries.GetById(request.SectionId) 
                           ?? throw new SectionNotFoundException(request.SectionId);

        var oldUser = await _userQueries.GetById(existingSection.UserId);
        var newUser = await _userQueries.GetById(request.UserId);

        // Оновлення даних секції
        existingSection.Name = request.Name;
        existingSection.UserId = request.UserId;
        existingSection.UpdatedAt = DateTime.UtcNow;

        await _sectionRepository.Update(request.SectionId, existingSection);

        // Видалення секції у старого користувача
        if (oldUser != null)
        {
            oldUser.Sections.RemoveAll(s => s.SectionId == existingSection.SectionId);
            await _userRepository.Update(oldUser.UserId, oldUser);
        }

        // Додавання секції до нового користувача
        if (newUser != null)
        {
            newUser.Sections.Add(existingSection);
            await _userRepository.Update(newUser.UserId, newUser);
        }

        return new SectionDto
        {
            SectionId = existingSection.SectionId,
            Name = existingSection.Name,
            UserId = existingSection.UserId,
            SectionItems = existingSection.SectionItems
        };
    }
}
