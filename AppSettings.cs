namespace mk_hangfire;


public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; } = null!;
    public ShopifyAccessInformation ShopifyAccessInformation { get; set; } = null!;

}

public class ConnectionStrings
{
    public required string DefaultConnection { get; set; }
}

public class ShopifyAccessInformation
{
    public required string Domain { get; set; }
    public required string AppPassKey { get; set; }
}