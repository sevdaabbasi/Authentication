namespace Authentication.Core.DTOs;

public class LoginRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
   // public string? RefreshToken { get; set; }
}