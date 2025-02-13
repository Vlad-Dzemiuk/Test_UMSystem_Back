using System.ComponentModel.DataAnnotations;
using Domain;

namespace API.Dtos;

public record SectionDto()
{
    public string SectionId { get; set; }
    public string Name { get; set; }
    public string? UserId { get; set; }
    public List<SectionItem> SectionItems { get; set; } = new();
}

public class CreateSectionDto
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string? UserId { get; set; }
}

public class UpdateSectionDto
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string? UserId { get; set; }
}