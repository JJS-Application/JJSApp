using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JJS.Application.Enums
{
    public enum FilterOptions 
    {
        [Display(Name = "StartsWith")]
        StartsWith = 1,

        [Display(Name = "EndsWith")]
        EndsWith,

        [Display(Name = "Contains")]
        Contains,

        [Display(Name = "DoesNotContain")]
        DoesNotContain,

        [Display(Name = "IsEmpty")]
        IsEmpty,

        [Display(Name = "IsNotEmpty")]
        IsNotEmpty,
        [Display(Name = "IsGreaterThan")]
        IsGreaterThan,

        [Display(Name = "IsGreaterThanOrEqualTo")]
        IsGreaterThanOrEqualTo,

        [Display(Name = "IsLessThan")]
        IsLessThan,

        [Display(Name = "IsLessThanOrEqualTo")]
        IsLessThanOrEqualTo,

        [Display(Name = "IsEqualTo")]
        IsEqualTo,

        [Display(Name = "IsNotEqualTo")]
        IsNotEqualTo
    }
}
