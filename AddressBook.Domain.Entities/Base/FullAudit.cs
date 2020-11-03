using System;

namespace AddressBook.Core.Entities.Base
{
    public class FullAudit
    {
        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool Active { get; set; } = true;
    }

    public interface IAuditFields
    {
        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }
    }
}
