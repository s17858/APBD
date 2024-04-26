namespace APBD;

public class Animal
{
    public int IdStudent { get; set; }
    [Required]
    [MaxLength(3)]
    public string Name { get; set; }
    [Required]
    [MaxLength(40)]
    public string Description { get; set; }
    [Required]
    [MaxLength(100)]
    public string Category { get; set; }
    [Required]
    [MaxLength(30)]
    public string Area { get; set; }

}
