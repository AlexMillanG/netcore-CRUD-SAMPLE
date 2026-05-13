using System.Text.Json.Serialization;

namespace SolucionChida.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    [JsonIgnore]
    public ICollection<Product> Products { get; set; } = new List<Product>();
}