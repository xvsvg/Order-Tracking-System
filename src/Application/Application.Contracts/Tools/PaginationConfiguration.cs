namespace Application.Contracts.Tools;

public class PaginationConfiguration
{
    public PaginationConfiguration(int size)
        => PageSize = size;
    
    public int PageSize { get; init; }
}