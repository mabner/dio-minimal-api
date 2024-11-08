using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApi.Domain.Entities;

public class Vehicle
{
    private string result;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } = default!;

    [Required]
    [StringLength(150)]
    public string Model { get; set; } = default!;

    [Required]
    [StringLength(100)]
    public string Make { get; set; } = default!;

    [Required]
    public int Year { get; set; } = default!;
}
