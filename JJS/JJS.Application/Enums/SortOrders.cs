using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JJS.Application.Enums
{
    [Flags]
    public enum SortOrders
    {
        [Display(Name ="Asc")]
        Asc = 1,
        [Display(Name = "Desc")]
        Desc = 2
    }
}
