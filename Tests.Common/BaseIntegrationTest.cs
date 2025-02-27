using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using Xunit;

namespace Tests.Common;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebFactory>
{
    protected readonly MongoDbService MongoDbService;
    protected readonly HttpClient Client;
    private readonly MongoDbContainer _mongoContainer = new MongoDbBuilder()
        .WithImage("mongo:latest")
        .WithPortBinding(27017, true)
        .Build();

    protected BaseIntegrationTest(IntegrationTestWebFactory factory)
    {
        _mongoContainer.StartAsync().Wait();
        var scope = factory.Services.CreateScope();
        
        MongoDbService = scope.ServiceProvider.GetRequiredService<MongoDbService>();
        Client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("TestScheme")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "TestScheme", _ => { });
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("TestScheme");
    }

    protected async Task ClearDatabaseAsync()
    {
        var database = MongoDbService.Database;
        var collections = await database.ListCollectionNamesAsync();
        foreach (var collection in await collections.ToListAsync())
        {
            await database.DropCollectionAsync(collection);
        }
    }
}

public class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] { new Claim(ClaimTypes.Role, "admin"), new Claim("userId", "admin") };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestScheme");

        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }
}
