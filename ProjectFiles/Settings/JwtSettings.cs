namespace AgroTechProject.Settings;

public class JwtSettings
{
    public required string Secret { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public int ExpiryInMinutes { get; set; }
    public int RefreshTokenExpiryInDays { get; set; } = 7;

}