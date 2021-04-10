using System;
using System.Collections.Generic;
using System.Text;

namespace JJS.Application.StaticMessages
{
    public static class Messages
    {
        public static class CompanyMessages
        {
            public static readonly string NotFound = "Company not found";
            public static readonly string Deleted = "Company deleted successfully";
            public static readonly string Update = "Company update successfully with Id: {0}";
            public static readonly string Added = "Company Added successfully with Id: {0}";
        }

        public static class BusinessStreamMessages
        {
            public static readonly string NotFound = "Business Stream not found";
            public static readonly string Deleted = "Business Stream deleted successfully";
            public static readonly string Update = "Business Stream update successfully with Id: {0}";
            public static readonly string Added = "Business Stream Added successfully with Id: {0}";
        }
    }
}
