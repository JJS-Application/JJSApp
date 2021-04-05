using System;
using System.Collections.Generic;
using System.Text;

namespace JJS.Application.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}
