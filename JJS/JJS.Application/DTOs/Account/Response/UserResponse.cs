using System;
using System.Collections.Generic;
using System.Text;

namespace JJS.Application.DTOs.Account.Response
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }       
        public bool? IsCompany { get; set; }
        public string ImageUrl { get; set; }
        public string FullName => FirstName + LastName;
        public string Email { get; set; }
        public virtual string PhoneNumber { get; set; }
    }
}
