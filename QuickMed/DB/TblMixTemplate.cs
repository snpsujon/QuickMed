using SQLite;

namespace QuickMed.DB
{
    public class TblMixTemplate
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Dose { get; set; }
        public string Duration { get; set; }
        public string TotalQty { get; set; }
        public string Instruction { get; set; }
        public string Notes { get; set; }


    }
}
