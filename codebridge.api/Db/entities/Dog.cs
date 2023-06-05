using Microsoft.EntityFrameworkCore;

namespace codebridge.api.Data.entities;

/// <summary>
/// requirement 1: dog's name is unque
/// </summary>
[Index(nameof(Name), IsUnique = true)]
public class Dog
{
    public Guid DogId { get; set; }

    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;

    public int TailLength { get; set; }
    
    public int Weight { get; set; }
}
