namespace mk_hangfire;


public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; } = null!;
}

public class ConnectionStrings
{
    public required string DefaultConnection { get; set; }
}
