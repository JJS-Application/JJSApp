using System;
using System.Collections.Generic;
using System.Text;

namespace JJS.Application.ViewModels.Company
{
    public class OrgViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? EstablishmentDate { get; set; }
        public string WebsiteURL { get; set; }
    }
}
