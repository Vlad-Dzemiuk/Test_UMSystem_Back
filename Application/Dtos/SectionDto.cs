using System.ComponentModel.DataAnnotations;
using Domain;

namespace API.Dtos;

public record SectionDto()
{
    public string SectionId { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
    public List<SectionItem> SectionItems { get; set; } = new();
}

public class CreateSectionDto
{
    public string Name { get; set; }
    public string UserId { get; set; }
}

public class UpdateSectionDto
{
    public string Name { get; set; }
    public string UserId { get; set; }
}