using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain;

[BsonIgnoreExtraElements]
public class SectionItem
{
    [BsonId]
    [BsonElement("section_item_id"), BsonRepresentation(BsonType.ObjectId)]
    public string? SectionItemId { get; set; }
    
    [BsonElement("_title"), BsonRepresentation(BsonType.String)]
    public string? Title { get; set; }
    
    [BsonElement("_content"), BsonRepresentation(BsonType.String)]
    public string? Content { get; set; }
    
    [BsonElement("created_at"), BsonRepresentation(BsonType.DateTime)]
    public DateTime CreatedAt { get; set; }
    
    [BsonElement("updated_at"), BsonRepresentation(BsonType.DateTime)]
    public DateTime UpdatedAt { get; set; }
    
    [BsonElement("section_id"), BsonRepresentation(BsonType.ObjectId)]
    public string? SectionId { get; set; }
    
    private SectionItem(string title, string content, DateTime createdAt, DateTime updatedAt, string? sectionId)
    {
        Title = title;
        Content = content;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        SectionId = sectionId;
    }
    
    public SectionItem()
    {
    }
}