using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJS.Infrastructure.Identity.Models
{
    public class BaseAudit : IBaseAudit
    {
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
    public interface IBaseAudit
    {
        DateTime Created { get; set; }
        DateTime? LastModified { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
    }
}
