namespace mk_booker_jobs;


public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; } = null!;
}

public class ConnectionStrings
{
    public required string DefaultConnection { get; set; }
}
