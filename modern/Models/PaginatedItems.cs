namespace EShop.Modern.Models;

// Mirrors legacy ViewModel/PaginatedItemsViewModel.
public class PaginatedItems<T>
{
    public PaginatedItems(int pageIndex, int pageSize, int totalItems, IReadOnlyList<T> data)
    {
        ActualPage = pageIndex;
        ItemsPerPage = pageSize;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
        Data = data;
    }

    public int ActualPage { get; }

    public int ItemsPerPage { get; }

    public int TotalItems { get; }

    public int TotalPages { get; }

    public IReadOnlyList<T> Data { get; }
}
