namespace SolucionChida.Domain.Entities;

public class User {
    public int Id { get; set; }
    public string name { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public DateTime CreatedAt{ get; set; }
    
    public string PasswordHashed { get; set; } = string.Empty;

    public ICollection<Role> Roles { get; set; } = new List<Role>();
    
    
}