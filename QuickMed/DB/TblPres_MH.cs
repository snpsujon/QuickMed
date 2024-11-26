using SQLite;

namespace QuickMed.DB
{
    public class TblPres_MH
    {
        [PrimaryKey]
        public Guid? Id { get; set; }
        public Guid? Pres_ID { get; set; }
        public string? MH { get; set; }
        public string? Value { get; set; }
    }
    public class TblPres_MHTemp
    {
        [PrimaryKey]
        public Guid? Id { get; set; }
        public Guid? Pres_ID { get; set; }
        public string? MH { get; set; }
        public string? Value { get; set; }
    }
}
