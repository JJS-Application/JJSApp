using System;
using System.Collections.Generic;
using System.Text;

namespace JJS.Application.DTOs.Company
{
    public class OrgDto
    {         
        public int Id { get; set; }
        public string Name { get; set; }        
        public string Description { get; set; }
        public DateTime? EstablishmentDate { get; set; }        
        public string WebsiteURL { get; set; }
    }
}
