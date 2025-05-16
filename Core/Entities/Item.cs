using System.Text.Json.Serialization;

namespace Core.Entities;

public class Item : BaseEntity
{
    public required string Name { get; set; }
    public required string Url { get; set; }
    public string ContentType { get; set; } = "application/pdf";
    
    public int FolderId { get; set; }
    [JsonIgnore] public Folder? Folder { get; set; }
}