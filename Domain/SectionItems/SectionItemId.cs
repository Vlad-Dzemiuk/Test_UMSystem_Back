namespace Domain.Section_Items;

public record SectionItemId(Guid Value)
{
    public static SectionItemId New() => new(Guid.NewGuid());
    public static SectionItemId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}