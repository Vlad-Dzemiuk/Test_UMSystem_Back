using System.ComponentModel.DataAnnotations;

namespace API.Dtos;

public record SectionItemDto()
{
    public string SectionItemId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string? SectionId { get; set; }    
}

public class CreateSectionItemDto
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }
    
    [Required]
    public string? SectionId { get; set; }
}

public class UpdateSectionItemDto
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }
    
    [Required]
    public string? SectionId { get; set; }
}