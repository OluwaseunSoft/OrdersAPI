using Microsoft.EntityFrameworkCore;

public static class IQueryableExtensions
{
    public static async Task<PagedResponse<T>> ToPagedResponseAsync<T>(this IQueryable<T> query, Pagination pagination)
    {
        var totalItems = await query.CountAsync();
        
        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync();
        return new PagedResponse<T>(items, pagination.PageNumber, pagination.PageSize, totalItems);
    }
}