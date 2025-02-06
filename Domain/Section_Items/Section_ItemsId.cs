namespace Domain.Section_Items;

public record Section_ItemsId(Guid Value)
{
    public static Section_ItemsId New() => new(Guid.NewGuid());
    public static Section_ItemsId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}