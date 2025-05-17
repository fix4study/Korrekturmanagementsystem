namespace Korrekturmanagementsystem.Models;

public class PagingModel
{
    public int PageSize { get; set; } = 5;
    public int CurrentPage { get; set; } = 1;
    public int TotalItems { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

    public void SetPage(int page)
    {
        if (page >= 1 && page <= TotalPages)
        {
            CurrentPage = page;
        }
    }

    public IEnumerable<T> Paginate<T>(IEnumerable<T> items)
    {
        return items.Skip((CurrentPage - 1) * PageSize).Take(PageSize);
    }
}
