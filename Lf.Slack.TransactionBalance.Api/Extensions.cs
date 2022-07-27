namespace Lf.Slack.TransactionBalance.Api;

public static class Extensions
{
    public static bool WithSwagger(this IWebHostEnvironment _)
    {
        if(!bool.TryParse(Environment.GetEnvironmentVariable("WITH_SWAGGER"), out var withSwagger))
        {
            return false;
        }

        return withSwagger;
    }
}