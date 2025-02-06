using Domain.Users;
using Domain.Section_Items;

namespace Domain.Sections;

public class Section
{
    public SectionId Id { get; }
    public string Name { get; private set; }
    public UserId UserId { get; }
    public User? User { get; }
    public List<Section_Items.Section_Items> SectionItemses { get; }
    public DateTime CreatedAt { get; private set; }
    
    private Section(SectionId id, string name, UserId userId, DateTime createdAt)
    {
        Id = id;
        Name = name;
        UserId = userId;
        CreatedAt = createdAt;
    }
    
    public static Section New(SectionId id, UserId userId, string name)
        => new(id, name, userId, DateTime.UtcNow);
}