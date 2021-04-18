using JJS.Domain.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JJS.Domain.Entities.CompanyTables
{
    public class BusinessStream : AuditableBaseEntity
    {
        public BusinessStream()
        {
            IsDeleted = false;
            IsActive = true;
        }
        public string Name { get; set; }

        [ForeignKey("Company")]
        public int? CompanyId { get; set; }
        public Organization Company { get; set; }

    }
}
