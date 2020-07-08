using System;
using System.Collections.Generic;

namespace HSM.WebApp.Data.Models
{
    public class Member
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? DOB { get; set; }
        public Gender? Gender { get; set; }
        
        public string OriginallyFrom { get; set; }
        public string Occupation { get; set; }
        
        public string PhoneNumber { get; set; }
        public string EMail { get; set; }

        public ICollection<Unit> OwnedUnits { get; set; }

        public DateTime? MemberFrom { get; set; } // Time when he/she was activated as member
        
        // ====== System Audit Data ======
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}