using API.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Sections.Exceptions;
using Domain;
using MediatR;

namespace Application.Sections.Commands;

public class CreateSectionCommand : IRequest<SectionDto>
{
    public string UserId { get; set; }
    public string Name { get; set; }
    
    public CreateSectionCommand(CreateSectionDto dto)
    {
        Name = dto.Name;
        UserId = dto.UserId;
    }
}

public class CreateSectionCommandHandler : IRequestHandler<CreateSectionCommand, SectionDto>
{
    private readonly ISectionRepository _sectionRepository;
    private readonly ISectionQueries _sectionQueries;
    private readonly IUserRepository _userRepository;
    private readonly IUserQueries _userQueries;

    public CreateSectionCommandHandler(ISectionRepository sectionRepository, ISectionQueries sectionQueries, IUserQueries userQueries, IUserRepository userRepository)
    {
        _sectionRepository = sectionRepository;
        _sectionQueries = sectionQueries;
        _userQueries = userQueries;
        _userRepository = userRepository;
    }

    public async Task<SectionDto> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
    {
        var existingSections = await _sectionQueries.GetAll();
        if (existingSections.Any(u => u.Name == request.Name))
        {
            throw new SectionAlreadyExistsException(request.Name);
        }

        var section = new Section
        {
            Name = request.Name,
            UserId = request.UserId,
            SectionItems = new List<SectionItem>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _sectionRepository.Create(section);

        var user = await _userQueries.GetById(request.UserId);
        if (user != null)
        {
            user.Sections.Add(section);
            await _userRepository.Update(user.UserId, user);
        }
        
        return new SectionDto
        {
            SectionId = section.SectionId,
            Name = section.Name,
            UserId = section.UserId,
            SectionItems = section.SectionItems
        };
    }
}