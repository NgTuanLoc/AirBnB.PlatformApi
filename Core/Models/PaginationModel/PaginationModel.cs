namespace Core.Models.PaginationModel;
public class PaginationModel
{
    public string? SortField { get; set; }
    public string? SortOrder { get; set; }
    public List<FilterDescriptor> FilterDescriptorList { get; set; } = new();
}

public class PagingParams
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 5;
}

public class FilterDescriptor
{
    public required string Value { get; set; }
    public required string Field { get; set; }
}

public class PaginationResponse<T>
{
    public List<T> Data { get; set; } = new List<T>();
    public int PageSize { get; set; }
    public int Page { get; set; }
    public double TotalRecords { get; set; }
    public double TotalFilteredRecords { get; set; }

}