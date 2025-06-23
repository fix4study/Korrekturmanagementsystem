namespace Korrekturmanagementsystem.Models;

public class PagingModel
{
    private int _currentPage = 1;
    private int _totalItems;

    public int PageSize { get; set; } = 5;

    public event Action? OnPagingChanged;

    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            if (_currentPage != value)
            {
                _currentPage = value;
                OnPagingChanged?.Invoke();
            }
        }
    }
    public int TotalItems
    {
        get => _totalItems;
        set
        {
            if (_totalItems != value)
            {
                _totalItems = value;
                OnPagingChanged?.Invoke();
            }
        }
    }

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
