using System.ComponentModel.DataAnnotations;

namespace Packt.Shared;

public class Category {
  // [Key]
  public int CategoryId { get; set; }

  [Required]
  [MaxLength(15)]
  public string CategoryName { get; set; } = null!;

  public string? Description { get; set; }
}