using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMed.DB
{

    public class TblDoctor
    {
        public Guid Id { get; set; } 
        public string? Name { get; set; } // Doctor's name
        public string? Mobile { get; set; } // Doctor's mobile number
        public string? Address { get; set; }
        public string? LicenseKey { get; set; }
        public string? CreatedBy { get; set; } // User who created the record
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp for creation
        public string? UpdatedBy { get; set; } // User who updated the record
        public DateTime? UpdatedAt { get; set; } // Timestamp for last update
        public bool IsSynced { get; set; } = false; // 0 = Unsynced, 1 = Synced
        public string? designation { get; set; }
        public string? department { get; set; }
        public string? passedMedical { get; set; }
        public string? bmdcRegNo { get; set; }
        public string? chemberHospital { get; set; }
        public string? phoneNumber { get; set; }
        public string? startTTime { get; set; }
        public string? offDay { get; set; }
        public string? endTime { get; set; }
    }

}
