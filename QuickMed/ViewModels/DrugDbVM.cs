using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMed.ViewModels
{
    public class DrugDbVM
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? GenericId { get; set; }
        public string? GenericName { get; set; }
        public Guid? ManufacturerId { get; set; }
        public string? CompanyName { get; set; }
        public string? PackageContainer { get; set; }
        public string? PackageSize { get; set; }
    }
}
