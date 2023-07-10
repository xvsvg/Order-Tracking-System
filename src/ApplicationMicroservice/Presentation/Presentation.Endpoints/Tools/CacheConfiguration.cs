namespace Presentation.Endpoints.Tools;

public class CacheConfiguration
{
    public CacheConfiguration(int timeInSeconds)
    {
        TimeInSeconds = timeInSeconds;
    }

    public int TimeInSeconds { get; init; }
}