using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain;
using MongoDB.Driver;

namespace Infrastructure.Persistence.Repositories;

public class SectionItemRepository : ISectionItemRepository, ISectionItemQueries
{
    private readonly IMongoCollection<SectionItem> _sectionItem;

    public SectionItemRepository(MongoDbService mongoDbService)
    {
        _sectionItem = mongoDbService.Database.GetCollection<SectionItem>("sectionItem");
    }

    public async Task<IEnumerable<SectionItem>> GetAll()
    {
        return await _sectionItem.Find(FilterDefinition<SectionItem>.Empty).ToListAsync();
    }

    public async Task<SectionItem> GetById(string id)
    {
        var filter = Builders<SectionItem>.Filter.Eq(x => x.SectionItemId, id);
        var sectionItem = await _sectionItem.Find(filter).FirstOrDefaultAsync();

        if (sectionItem == null)
        {
            throw new KeyNotFoundException($"Section item with id {id} not found.");
        }

        return sectionItem;
    }

    public async Task Create(SectionItem sectionItem)
    {
        await _sectionItem.InsertOneAsync(sectionItem);
    }

    public async Task Update(string id, SectionItem sectionItem)
    {
        var filter = Builders<SectionItem>.Filter.Eq(x => x.SectionItemId, id);
        await _sectionItem.ReplaceOneAsync(filter, sectionItem);
    }

    public async Task Delete(string id)
    {        
        var filter = Builders<SectionItem>.Filter.Eq(x => x.SectionItemId, id);
        await _sectionItem.DeleteOneAsync(filter);
    }
}