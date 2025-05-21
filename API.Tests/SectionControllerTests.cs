using System.Net;
using System.Net.Http.Json;
using Domain;
using FluentAssertions;
using Xunit;

namespace API.Tests;

public class SectionControllerTests : BaseIntegrationTest
{
    [Fact]
    public async Task GetSections_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/sections");
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task GetAll_ReturnsOkResult_WithSections()
    {
        // Arrange
        var section1 = new Section { Name = "Section 1", UserId = "60c728e578c2e64b2c8a7b9a", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        var section2 = new Section { Name = "Section 2", UserId = "60c728e578c2e64b2c8a7b9b", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        await _mongoDatabase.GetCollection<Section>("Sections").InsertManyAsync(new[] { section1, section2 });

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/Section");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var sections = await response.Content.ReadFromJsonAsync<IEnumerable<Section>>();
        sections.Should().NotBeNullOrEmpty();
        sections.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_ReturnsOkResult_WithSection()
    {
        // Arrange
        var section = new Section { Name = "Test Section", UserId = "60c728e578c2e64b2c8a7b9c", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        await _mongoDatabase.GetCollection<Section>("Sections").InsertOneAsync(section);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/Section/{section.SectionId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var returnedSection = await response.Content.ReadFromJsonAsync<Section>();
        returnedSection.Should().NotBeNull();
        returnedSection.SectionId.Should().Be(section.SectionId);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenSectionDoesNotExist()
    {
        // Arrange
        var nonExistentId = "60c728e578c2e64b2c8a7b9d"; // Випадковий ID, що не існує

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/Section/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}