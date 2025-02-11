using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Users;
using MongoDB.Driver;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository, IUserQueries
{
    private readonly IMongoCollection<User> _user;

    public UserRepository(MongoDbService mongoDbService)
    {
        _user = mongoDbService.Database.GetCollection<User>("user");
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _user.Find(FilterDefinition<User>.Empty).ToListAsync();
    }

    public async Task<User> GetById(string id)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Id.Value, Guid.Parse(id));
        var user = await _user.Find(filter).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }

        return user;
    }

    public async Task Create(User user)
    {
        await _user.InsertOneAsync(user);
    }

    public async Task Update(string id, User user)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Id.Value, Guid.Parse(id));
        await _user.ReplaceOneAsync(filter, user);
    }

    public async Task Delete(string id)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Id.Value, Guid.Parse(id));
        await _user.DeleteOneAsync(filter);
    }
}