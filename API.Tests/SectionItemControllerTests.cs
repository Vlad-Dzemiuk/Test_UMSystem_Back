using API.Controllers;
using API.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.SectionItems.Commands;
using Application.SectionItems.Exceptions;
using Domain;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace API.Tests;

public class SectionItemControllerTests : BaseIntegrationTest
{
    private readonly Mock<ISectionItemRepository> _sectionItemRepository = new();
    private readonly Mock<ISectionItemQueries> _sectionItemQueries = new();
    private readonly Mock<IMediator> _mediator = new();

    private readonly SectionItemController _controller;

    public SectionItemControllerTests()
    {
        _controller = new SectionItemController(
            _sectionItemRepository.Object,
            _sectionItemQueries.Object,
            _mediator.Object
        );
    }

    [Fact]
    public async Task GetAll_ReturnsAllSectionItems()
    {
        var items = new List<SectionItem>
        {
            new() { Title = "Item A", SectionId = "sec1" },
            new() { Title = "Item B", SectionId = "sec2" }
        };

        _sectionItemQueries.Setup(q => q.GetAll()).ReturnsAsync(items);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedItems = Assert.IsAssignableFrom<IEnumerable<SectionItem>>(okResult.Value);
        Assert.Equal(2, returnedItems.Count());
    }

    [Fact]
    public async Task GetById_ReturnsSectionItem_WhenFound()
    {
        var id = ObjectId.GenerateNewId().ToString();
        var item = new SectionItem { SectionItemId = id, Title = "Item", SectionId = "sec1" };

        _sectionItemQueries.Setup(q => q.GetById(id)).ReturnsAsync(item);

        var result = await _controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedItem = Assert.IsType<SectionItem>(okResult.Value);
        Assert.Equal(id, returnedItem.SectionItemId);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        var id = ObjectId.GenerateNewId().ToString();

        _sectionItemQueries.Setup(q => q.GetById(id))
            .ThrowsAsync(new KeyNotFoundException("Опис секції не знайдено"));

        var result = await _controller.GetById(id);

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Опис секції не знайдено", notFound.Value);
    }

    [Fact]
    public async Task Post_CreatesSectionItem_ReturnsCreatedItem()
    {
        var dto = new CreateSectionItemDto
        {
            Title = "Test Item",
            Content = "Some content",
            SectionId = ObjectId.GenerateNewId().ToString()
        };

        var expected = new SectionItemDto
        {
            SectionItemId = ObjectId.GenerateNewId().ToString(),
            Title = dto.Title,
            Content = dto.Content,
            SectionId = dto.SectionId
        };

        _mediator.Setup(m => m.Send(It.IsAny<CreateSectionItemsCommand>(), default))
            .ReturnsAsync(expected);

        var result = await _controller.Post(dto);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returned = Assert.IsType<SectionItemDto>(created.Value);
        Assert.Equal(dto.Title, returned.Title);
        Assert.Equal(dto.SectionId, returned.SectionId);
    }

    [Fact]
    public async Task Post_WhenSectionItemAlreadyExists_ReturnsBadRequest()
    {
        var dto = new CreateSectionItemDto
        {
            Title = "Existing Item",
            Content = "Content",
            SectionId = "sec1"
        };

        _mediator.Setup(m => m.Send(It.IsAny<CreateSectionItemsCommand>(), default))
            .ThrowsAsync(new SectionItemAlreadyExistsException(dto.Title));

        var result = await _controller.Post(dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        badRequest.Value.Should().Be($"Опис секції з назвою \"{dto.Title}\" вже існує.");
    }

    [Fact]
    public async Task Put_UpdatesSectionItem_ReturnsUpdatedItem()
    {
        var id = ObjectId.GenerateNewId().ToString();
        var dto = new UpdateSectionItemDto
        {
            Title = "Updated Title",
            Content = "Updated Content"
        };

        var expected = new SectionItemDto
        {
            SectionItemId = id,
            Title = dto.Title,
            Content = dto.Content,
            SectionId = "sec1"
        };

        _mediator.Setup(m => m.Send(It.IsAny<UpdateSectionItemsCommand>(), default))
            .ReturnsAsync(expected);

        var result = await _controller.Put(id, dto);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsType<SectionItemDto>(ok.Value);
        Assert.Equal(dto.Title, returned.Title);
        Assert.Equal(dto.Content, returned.Content);
    }

    [Fact]
    public async Task Put_WhenSectionItemNotFound_ReturnsNotFound()
    {
        var id = ObjectId.GenerateNewId().ToString();
        var dto = new UpdateSectionItemDto
        {
            Title = "Title",
            Content = "Content"
        };

        _mediator.Setup(m => m.Send(It.IsAny<UpdateSectionItemsCommand>(), default))
            .ThrowsAsync(new KeyNotFoundException("Опис секції не знайдено"));

        var result = await _controller.Put(id, dto);

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        notFound.Value.Should().Be("Опис секції не знайдено");
    }

    [Fact]
    public async Task Delete_RemovesSectionItem_ReturnsOk()
    {
        var id = ObjectId.GenerateNewId().ToString();

        _mediator.Setup(m => m.Send(It.IsAny<DeleteSectionItemsCommand>(), default))
            .ReturnsAsync(Unit.Value);

        var result = await _controller.Delete(id);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Delete_WhenSectionItemNotFound_ReturnsNotFound()
    {
        var id = ObjectId.GenerateNewId().ToString();

        _mediator.Setup(m => m.Send(It.IsAny<DeleteSectionItemsCommand>(), default))
            .ThrowsAsync(new KeyNotFoundException("Опис секції не знайдено"));

        var result = await _controller.Delete(id);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        notFound.Value.Should().Be("Опис секції не знайдено");
    }
}