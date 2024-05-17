using Assignment_API.DataModals;
using System.Linq.Expressions;

namespace Assignment_API.Models.Dto
{
    public class PaginationDTO
    {
        public string? searchQuery { get; set; } = null;
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public bool isAcending { get; set; } = true;

    }
}
