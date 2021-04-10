using JJS.Application.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace JJS.Application.FilterSorting
{
    /// <summary>  
    /// Filter parameters Model Class  
    /// </summary>  
    public class FilterParams
    {
        public string ColumnName { get; set; } = string.Empty;
        public string FilterValue { get; set; } = string.Empty;
        public FilterOptions FilterOption { get; set; } = FilterOptions.Contains;
    }
}
