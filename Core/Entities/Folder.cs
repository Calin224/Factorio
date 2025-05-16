using System.Text.Json.Serialization;

namespace Core.Entities;

public class Folder : BaseEntity
{
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Item> Items { get; set; } = [];

    public string AppUserId { get; set; } = string.Empty;
    [JsonIgnore]
    public AppUser AppUser { get; set; } = null!;
}