namespace backendnet.Models;

public class CustomIdentityUserDTO{
    public string? id { get; set;}
    public required string Email {get; set;}
    public required string Nombre {get; set;}
    public required string Rol {get; set;}
}