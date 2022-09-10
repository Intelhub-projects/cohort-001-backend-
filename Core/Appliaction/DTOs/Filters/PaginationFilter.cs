﻿namespace Application.DTOs.Filters
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string[] OrderBy { get; set; }
    }
}
