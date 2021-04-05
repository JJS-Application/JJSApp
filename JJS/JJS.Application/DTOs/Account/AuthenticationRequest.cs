using System;
using System.Collections.Generic;
using System.Text;

namespace JJS.Application.DTOs.Account
{
    public class AuthenticationRequest
    {
        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}
