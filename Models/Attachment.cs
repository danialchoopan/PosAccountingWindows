using System.ComponentModel.DataAnnotations;

namespace PosAccountingApp.Models;

public class Attachment : BaseEntity
{
    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public string? EntityType { get; set; }

    public int? EntityId { get; set; }

    public string? Description { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.Now;

    public byte[]? FileData { get; set; }
}
