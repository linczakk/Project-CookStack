using CookStack.Client.Enums;

namespace CookStack.Client.Models.Sort
{
    public class SortOptions<TSortField>
    {
        public TSortField Field { get; set; } = default!;
        public SortDirection Direction { get; set; }
    }
}
