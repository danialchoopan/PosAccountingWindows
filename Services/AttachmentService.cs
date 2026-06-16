using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.Services;

public class AttachmentService
{
    public Attachment SaveAttachment(string fileName, byte[] fileData, string contentType,
        string? entityType = null, int? entityId = null, string? description = null)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        var attachment = new Attachment
        {
            FileName = fileName,
            FilePath = $"attachments/{DateTime.Now:yyyyMMdd}/{fileName}",
            ContentType = contentType,
            FileSize = fileData.Length,
            EntityType = entityType,
            EntityId = entityId,
            Description = description,
            FileData = fileData,
            UploadedAt = DateTime.Now
        };
        db.Attachments.Add(attachment);
        db.SaveChanges();
        return attachment;
    }

    public List<Attachment> GetAttachments(string entityType, int entityId)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        return db.Attachments.AsNoTracking()
            .Where(a => a.IsActive && a.EntityType == entityType && a.EntityId == entityId)
            .OrderByDescending(a => a.UploadedAt)
            .ToList();
    }

    public Attachment? GetAttachment(int id)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        return db.Attachments.Find(id);
    }

    public void DeleteAttachment(int id)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        var attachment = db.Attachments.Find(id);
        if (attachment != null)
        {
            attachment.IsActive = false;
            db.SaveChanges();
        }
    }

    public List<Attachment> GetAllAttachments()
    {
        using var db = DatabaseInitializer.CreateDbContext();
        return db.Attachments.AsNoTracking()
            .Where(a => a.IsActive)
            .OrderByDescending(a => a.UploadedAt)
            .ToList();
    }
}
