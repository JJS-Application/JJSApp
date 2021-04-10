using JJS.Application.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace JJS.Application.FilterSorting
{
    public class SortingParams
    {
        public SortOrders SortOrder { get; set; } = SortOrders.Asc;
        public string ColumnName { get; set; }
    }
}
