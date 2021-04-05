using System;
using System.Collections.Generic;
using System.Text;

namespace JJS.Application.Interfaces
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }
    }
}
