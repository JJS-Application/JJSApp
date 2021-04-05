using JJS.Domain.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  JJS.Domain.Entities.CompanyTables
{
    [Table("Company")]
    public class Organization : AuditableBaseEntity
    {
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EstablishmentDate { get; set; }

        [StringLength(255)]
        public string WebsiteURL { get; set; }
        public List<BusinessStream> BusinessStream { get; set; }
    }
}
