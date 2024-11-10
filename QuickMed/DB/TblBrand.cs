using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMed.DB
{
    public class TblBrand
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? GenericId { get; set; }
        public Guid? CompanyId { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Use UTC time for consistency
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public bool IsSynced { get; set; } = false; // Default to false for new records

        [NotMapped]
        public string GenericName { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }


    }
}
