using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Sections;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class SectionRepository(ApplicationDbContext context) : ISectionRepository, ISectionQueries
{
    public async Task<IReadOnlyList<Section>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Sections
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Section> Create(Section section, CancellationToken cancellationToken)
    {
        await context.Sections.AddAsync(section, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return section;
    }

    public async Task<Option<Section>> GetById(SectionId id, CancellationToken cancellationToken)
    {
        var entity = await context.Sections
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Section>() : Option.Some(entity);
    }

    public async Task<Section> Update(Section section, CancellationToken cancellationToken)
    {
        context.Sections.Update(section);

        await context.SaveChangesAsync(cancellationToken);

        return section;
    }

    public async Task<Section> Delete(Section section, CancellationToken cancellationToken)
    {
        context.Sections.Remove(section);

        await context.SaveChangesAsync(cancellationToken);

        return section;
    }
}