using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMed.DB
{
    public class TblMixTempDetails
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string BrandId { get; set; }
        public string DoseId { get; set; }
        public Guid TblMixTemplateMasterId { get; set; }
    }
}
