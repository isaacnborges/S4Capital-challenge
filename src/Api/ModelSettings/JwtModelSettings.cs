namespace S4Capital.Api.Api.ModelSettings;

public class JwtModelSettings
{
    public string Secret { get; set; }
    public int Expires { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
