namespace Core.Entities;

public class Folder : BaseEntity
{
    public string Name { get; set; }
    public List<Item> Items { get; set; } = [];

    public string AppUserId { get; set; } = string.Empty;
}