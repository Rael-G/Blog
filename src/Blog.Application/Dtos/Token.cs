namespace Blog.Application;

public class Token
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime Creation { get; set; }
    public DateTime Expiration { get; set; }
    public string RefreshToken { get; set; }  = string.Empty;
}