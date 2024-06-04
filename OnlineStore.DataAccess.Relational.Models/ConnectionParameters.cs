namespace OnlineStore.DataAccess.Relational.Models;

public sealed class ConnectionParameters()
{
    public required string Provider { get; set; }

    public required string Server { get; set; }

    public required int Port { get; set; }

    public required string Database { get; set; }

    public bool IntegratedSecurity { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public required bool TrustedConnection { get; set; }

    public required bool TrustServerCertificate { get; set; }

    public TimeSpan? ConnectionTimeout { get; set; }

    public int? MaxPoolSize { get; set; }
}