namespace mk_hangfire;


public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; } = null!;
    public ShopifyAccessInformation ShopifyAccessInformation { get; set; } = null!;
    public SmartPackAccessInformation SmartPack { get; set; } = null;

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

public class SmartPackAccessInformation
{
    public required string APIUrl { get; set; }
    public required string AppId { get; set; }
    public required string AppToken { get; set; }
}