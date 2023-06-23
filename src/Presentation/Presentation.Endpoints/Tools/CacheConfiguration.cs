namespace Presentation.Endpoints.Tools;

public class CacheConfiguration
{
    public CacheConfiguration(int time)
        => TimeInSeconds = time;
    
    public int TimeInSeconds { get; init; }
}