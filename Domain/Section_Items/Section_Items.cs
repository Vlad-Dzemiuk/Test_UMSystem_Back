using Domain.Sections;
using Domain.Users;

namespace Domain.Section_Items;

public class Section_Items
{
    public Section_ItemsId Id { get; }
    public SectionId SectionId { get; }
    public Section? Section { get; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    private Section_Items(Section_ItemsId id, string title, string content, SectionId sectionId, DateTime createdAt)
    {
        Id = id;
        Title = title;
        Content = content;
        SectionId = sectionId;
        CreatedAt = createdAt;
    }
    
    public static Section_Items New(Section_ItemsId id, SectionId sectionId, string title, string content)
        => new(id, title, content, sectionId, DateTime.UtcNow);
}