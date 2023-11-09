using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Models.PaginationModel;

namespace Core.Utils.ServerSidePaginationUtils;
public static class RoomPaginateUtil
{
    public enum SortOrderType
    {
        DESC = 0,
        ASC = 1
    }
    public static IQueryable<Room> ApplyFilters(IQueryable<Room> query, List<FilterDescriptor> filterDescriptors)
    {
        if (filterDescriptors.Count == 0) return query;

        Dictionary<string, Func<(string, IQueryable<Room>), IQueryable<Room>>> filterQueries = new()
        {
            {nameof(Room.Name).ToUpper(), data => data.Item2.Where(item => item.Name.Contains(data.Item1))}
        };

        foreach (var filter in filterDescriptors)
        {
            var key = filter.Field.ToUpper();
            var value = filter.Value.ToUpper();
            query = filterQueries[key]((value, query));
        }

        return query;
    }

    public static IQueryable<Room> ApplySorting(IQueryable<Room> query, string? sortField, string? sortOrder)
    {
        if (string.IsNullOrEmpty(sortField) || string.IsNullOrEmpty(sortOrder))
        {
            return query;
        }

        Dictionary<string, Expression<Func<Room, object>>> SortParameters = new()
        {
            {nameof(Room.Name).ToUpper(), item => item.Name}
        };

        if (sortOrder.ToUpper() == Enum.GetName(SortOrderType.DESC))
        {
            return query.OrderByDescending(SortParameters[sortField.ToUpper()]);
        }

        if (sortOrder.ToUpper() == Enum.GetName(SortOrderType.ASC))
        {
            return query.OrderBy(SortParameters[sortField.ToUpper()]);
        }

        return query;
    }

    public static IQueryable<Room> ApplyPagination(IQueryable<Room> query, PagingParams pagingParams)
    {
        var skip = (pagingParams.Page - 1) * pagingParams.PageSize;
        return query.Skip(skip).Take(pagingParams.PageSize);
    }
}