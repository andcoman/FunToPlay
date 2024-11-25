using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FunToPlay.Infrastructure.Data.Entities;

public class ResourceDbEntity
{
    public Guid ResourceId { get; set; }

    [Required]
    public Guid PlayerId { get; set; }

    [ForeignKey(nameof(PlayerId))]
    public PlayerDbEntity Player { get; set; }

    [Required]
    [MaxLength(20)]
    public string ResourceType { get; set; }
    
    public int ResourceValue { get; set; }
}
