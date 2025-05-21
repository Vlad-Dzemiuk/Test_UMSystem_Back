using Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using API;
using Xunit;

namespace API.Tests;

public class BaseIntegrationTest : IAsyncLifetime
{
    protected readonly WebApplicationFactory<global::Program> _factory;
    protected IMongoDatabase _mongoDatabase;
    protected HttpClient _client;

    private readonly string _mongoConnectionString = "mongodb://localhost:27017";
    private readonly string _databaseName = "test_db";

    public BaseIntegrationTest()
    {
        _factory = new WebApplicationFactory<global::Program>();
        _client = _factory.CreateClient();

        var mongoClient = new MongoClient(_mongoConnectionString);
        _mongoDatabase = mongoClient.GetDatabase(_databaseName);
    }

    public virtual async Task InitializeAsync()
    {
        var collections = await _mongoDatabase.ListCollectionNamesAsync();
        foreach (var collection in await collections.ToListAsync())
        {
            await _mongoDatabase.DropCollectionAsync(collection);
        }
    }

    public virtual Task DisposeAsync()
    {
        _client.Dispose();
        return Task.CompletedTask;
    }
}