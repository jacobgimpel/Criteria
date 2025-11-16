using Microsoft.Extensions.Configuration;
using Criteria.Utilities;

public static class Secrets
{
    private static IConfiguration? _config;

    static Secrets()
    {
        _config = new ConfigurationBuilder()
            .AddUserSecrets<SecretHandler>()
            .Build();
    }
    public static string? TmdbApiKey =>
        _config["TMDB: ApiKey"];
}