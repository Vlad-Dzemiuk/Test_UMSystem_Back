using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain;
using MongoDB.Driver;

namespace Infrastructure.Persistence.Repositories;

public class SectionRepository : ISectionRepository, ISectionQueries
{
    private readonly IMongoCollection<Section> _section;

    public SectionRepository(MongoDbService mongoDbService)
    {
        _section = mongoDbService.Database.GetCollection<Section>("section");
    }

    public async Task<IEnumerable<Section>> GetAll()
    {
        return await _section.Find(FilterDefinition<Section>.Empty).ToListAsync();
    }

    public async Task<Section> GetById(string id)
    {
        var filter = Builders<Section>.Filter.Eq(x => x.SectionId, id);
        var section = await _section.Find(filter).FirstOrDefaultAsync();

        if (section == null)
        {
            throw new KeyNotFoundException($"Section with id {id} not found.");
        }

        return section;
    }

    public async Task Create(Section section)
    {
        await _section.InsertOneAsync(section);
    }

    public async Task Update(string id, Section section)
    {
        var filter = Builders<Section>.Filter.Eq(x => x.SectionId, id);
        await _section.ReplaceOneAsync(filter, section);
    }

    public async Task Delete(string id)
    {
        var filter = Builders<Section>.Filter.Eq(x => x.SectionId, id);
        await _section.DeleteOneAsync(filter);
    }
}