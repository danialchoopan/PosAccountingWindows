using System.ComponentModel.DataAnnotations;

namespace PosAccountingApp.Models;

public class ProductCategory : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Icon { get; set; } = "\uE74C";

    public int SortOrder { get; set; }

    public BusinessProfile Profile { get; set; }
}
