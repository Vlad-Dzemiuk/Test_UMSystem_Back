using API.Controllers;
using API.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Sections.Commands;
using Application.Sections.Exceptions;
using Domain;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace API.Tests;

public class SectionControllerTests : BaseIntegrationTest
{
    private readonly Mock<ISectionRepository> _sectionRepository = new();
    private readonly Mock<ISectionQueries> _sectionQueries = new();
    private readonly Mock<IMediator> _mediator = new();

    private readonly SectionController _sectionController;

    public SectionControllerTests()
    {
        _sectionController = new SectionController(
            _sectionRepository.Object,
            _sectionQueries.Object,
            _mediator.Object
        );
    }
    
    [Fact]
    public async Task GetAll_ReturnsAllSections()
    {
        // Arrange
        var sections = new List<Section>
        {
            new Section { Name = "Section A", UserId = "user1" },
            new Section { Name = "Section B", UserId = "user2" }
        };

        _sectionQueries.Setup(q => q.GetAll())
            .ReturnsAsync(sections);

        // Act
        var result = await _sectionController.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedSections = Assert.IsAssignableFrom<IEnumerable<Section>>(okResult.Value);
        Assert.Equal(2, returnedSections.Count());
    }

    [Fact]
    public async Task GetById_ReturnsSection_WhenFound()
    {
        // Arrange
        var id = ObjectId.GenerateNewId().ToString();
        var section = new Section { SectionId = id, Name = "My Section", UserId = "user1" };

        _sectionQueries.Setup(q => q.GetById(id))
            .ReturnsAsync(section);

        // Act
        var result = await _sectionController.GetById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedSection = Assert.IsType<Section>(okResult.Value);
        Assert.Equal(id, returnedSection.SectionId);
    }
    
    [Fact]
    public async Task GetById_ReturnsNotFound_WhenSectionNotExists()
    {
        // Arrange
        var id = ObjectId.GenerateNewId().ToString();

        _sectionQueries.Setup(q => q.GetById(id))
            .ThrowsAsync(new KeyNotFoundException("Section not found"));

        // Act
        var result = await _sectionController.GetById(id);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Section not found", notFoundResult.Value);
    }
    
    [Fact]
    public async Task Post_CreatesSection_ReturnsCreatedSection()
    {
        // Arrange
        var dto = new CreateSectionDto
        {
            Name = "Test Section",
            UserId = ObjectId.GenerateNewId().ToString()
        };

        var expectedSection = new SectionDto
        {
            SectionId = ObjectId.GenerateNewId().ToString(),
            Name = dto.Name,
            UserId = dto.UserId
        };

        _mediator
            .Setup(m => m.Send(It.IsAny<CreateSectionCommand>(), default(CancellationToken)))
            .ReturnsAsync(expectedSection);

        // Act
        var result = await _sectionController.Post(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedDto = Assert.IsType<SectionDto>(createdResult.Value);
        Assert.Equal(dto.Name, returnedDto.Name);
        Assert.Equal(dto.UserId, returnedDto.UserId);
    }
    
    [Fact]
    public async Task Post_WhenSectionAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        var dto = new CreateSectionDto
        {
            Name = "Existing Section",
            UserId = "user1"
        };

        _mediator.Setup(m => m.Send(It.IsAny<CreateSectionCommand>(), default))
            .ThrowsAsync(new SectionAlreadyExistsException(dto.Name));

        // Act
        var result = await _sectionController.Post(dto);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        badRequest.Value.Should().Be($"Секція з назвою \"{dto.Name}\" вже існує.");
    }
    
    [Fact]
    public async Task Put_UpdatesSection_ReturnsUpdatedSection()
    {
        // Arrange
        var id = ObjectId.GenerateNewId().ToString();
        var dto = new UpdateSectionDto
        {
            Name = "Updated Section",
            UserId = "user123"
        };

        var updatedSection = new SectionDto
        {
            SectionId = id,
            Name = dto.Name,
            UserId = dto.UserId,
            SectionItems = new List<SectionItem>()
        };

        _mediator.Setup(m => m.Send(It.IsAny<UpdateSectionCommand>(), default))
            .ReturnsAsync(updatedSection);

        // Act
        var result = await _sectionController.Put(id, dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDto = Assert.IsType<SectionDto>(okResult.Value);
        Assert.Equal(updatedSection.Name, returnedDto.Name);
        Assert.Equal(updatedSection.UserId, returnedDto.UserId);
    }
    
    [Fact]
    public async Task Put_WhenSectionNotFound_ReturnsNotFound()
    {
        // Arrange
        var id = ObjectId.GenerateNewId().ToString();
        var dto = new UpdateSectionDto
        {
            Name = "Updated Name",
            UserId = "user1"
        };

        _mediator.Setup(m => m.Send(It.IsAny<UpdateSectionCommand>(), default))
            .ThrowsAsync(new KeyNotFoundException("Секцію не знайдено"));

        // Act
        var result = await _sectionController.Put(id, dto);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Секцію не знайдено", notFound.Value);
    }


    [Fact]
    public async Task Delete_RemovesSection_ReturnsOk()
    {
        // Arrange
        var id = ObjectId.GenerateNewId().ToString();

        _mediator.Setup(m => m.Send(It.IsAny<DeleteSectionCommand>(), default))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _sectionController.Delete(id);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Delete_WhenSectionNotFound_ReturnsNotFound()
    {
        // Arrange
        var id = ObjectId.GenerateNewId().ToString();

        _mediator.Setup(m => m.Send(It.IsAny<DeleteSectionCommand>(), default))
            .ThrowsAsync(new KeyNotFoundException("Секція не знайдена"));

        // Act
        var result = await _sectionController.Delete(id);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        notFound.Value.Should().Be("Секція не знайдена");
    }
}