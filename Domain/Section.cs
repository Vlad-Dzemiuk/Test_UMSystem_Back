using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain;

[BsonIgnoreExtraElements]
public class Section
{
    [BsonId]
    [BsonElement("section_id"), BsonRepresentation(BsonType.ObjectId)]
    public string? SectionId { get; set; }
    
    [BsonElement("Name"), BsonRepresentation(BsonType.String)]
    public string? Name { get; set; }

    [BsonElement("user_id"), BsonRepresentation(BsonType.String)]
    public string? UserId { get; set; }
    
    [BsonElement("created_at"), BsonRepresentation(BsonType.DateTime)]
    public DateTime? CreatedAt { get; set; }
    
    [BsonElement("updated_at"), BsonRepresentation(BsonType.DateTime)]
    public DateTime? UpdatedAt { get; set; }

    [BsonIgnoreIfNull]
    public List<SectionItem>? SectionItems { get; set; } = new List<SectionItem>();

    private Section(string name, string? userId, DateTime createdAt, DateTime updatedAt)
    {
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        UserId = userId;
    }
    
    public Section()
    {
    }
}