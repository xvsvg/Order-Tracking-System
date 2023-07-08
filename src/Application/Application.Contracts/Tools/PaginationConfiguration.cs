namespace Application.Contracts.Tools;

public class PaginationConfiguration
{
    public PaginationConfiguration(int pageSize)
        => PageSize = pageSize;
    
    public int PageSize { get; init; }
}