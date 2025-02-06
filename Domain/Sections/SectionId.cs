namespace Domain.Sections;

public record SectionId(Guid Value)
{
    public static SectionId New() => new(Guid.NewGuid());
    public static SectionId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}