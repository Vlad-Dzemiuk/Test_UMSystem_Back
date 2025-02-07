using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Section_Items;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class Section_ItemsRepository(ApplicationDbContext context) : ISection_ItemsRepository, ISection_ItemsQueries
{
    public async Task<IReadOnlyList<Section_Items>> GetAll(CancellationToken cancellationToken)
    {
        return await context.SectionItems
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Section_Items> Create(Section_Items section_Items, CancellationToken cancellationToken)
    {
        await context.SectionItems.AddAsync(section_Items, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return section_Items;
    }

    public async Task<Option<Section_Items>> GetById(Section_ItemsId id, CancellationToken cancellationToken)
    {
        var entity = await context.SectionItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Section_Items>() : Option.Some(entity);
    }

    public async Task<Section_Items> Update(Section_Items section_Items, CancellationToken cancellationToken)
    {
        context.SectionItems.Update(section_Items);

        await context.SaveChangesAsync(cancellationToken);

        return section_Items;
    }

    public async Task<Section_Items> Delete(Section_Items section_Items, CancellationToken cancellationToken)
    {
        context.SectionItems.Remove(section_Items);

        await context.SaveChangesAsync(cancellationToken);

        return section_Items;
    }
}