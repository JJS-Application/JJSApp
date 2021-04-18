using JJS.Application.DTOs.Account;

using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Text;

namespace JJS.Infrastructure.Identity.Models
{
    public class ApplicationUser : IdentityUser, IBaseAudit
    {
        public ApplicationUser()
        {
            IsActive = true;
            IsDeleted = false;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsCompany { get; set; }
        public string ImageUrl { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}
