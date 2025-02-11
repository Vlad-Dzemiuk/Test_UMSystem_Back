using Domain.Sections;
using Domain.Users;

namespace Domain.Section_Items;

public class SectionItem
{
    public SectionItemId Id { get; }
    public SectionId SectionId { get; }
    public Section? Section { get; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    private SectionItem(SectionItemId id, string title, string content, SectionId sectionId, DateTime createdAt)
    {
        Id = id;
        Title = title;
        Content = content;
        SectionId = sectionId;
        CreatedAt = createdAt;
    }
    
    public static SectionItem New(SectionItemId id, SectionId sectionId, string title, string content)
        => new(id, title, content, sectionId, DateTime.UtcNow);
}