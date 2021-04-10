using JJS.Application.FilterSorting;

using System;
using System.Collections.Generic;
using System.Text;

namespace JJS.Application.ViewModels.RequestFilter
{
    public class RequestFilterViewModel
    {
        public RequestFilterViewModel()
        {
            if (SortingParams == null)
            {
                SortingParams = new List<SortingParams>
                {
                    new SortingParams {
                        ColumnName = "Created",
                        SortOrder = Enums.SortOrders.Desc
                    }
                };
            }
        }
        public List<SortingParams> SortingParams { set; get; }
        public List<FilterParams> FilterParam { get; set; }
        public List<string> GroupingColumns { get; set; } = null;
        public int pageNumber = 1;
        public int PageNumber { get { return pageNumber; } set { if (value > 1) pageNumber = value; } }

        public int pageSize = 10;
        public int PageSize { get { return pageSize; } set { if (value > 1) pageSize = value; } }
    }
}
