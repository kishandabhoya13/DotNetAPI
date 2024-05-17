using System.Linq.Expressions;

namespace Assignment.Models.Dto
{
    public class PaginationDTO
    {
        public string? searchQuery { get; set; } = null;

        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        public int ItemsPerPage { get; set; } = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public bool isAcending { get; set; } = true;

    }
}
