using SQLite;

namespace QuickMed.DB
{
    public class TblPres_OE
    {
        [PrimaryKey]
        public Guid? Id { get; set; }
        public Guid? Pres_ID { get; set; }
        public string? Name { get; set; }
        public string? value { get; set; }
        public string? Unit { get; set; }
    }
    public class TblPres_OETemp
    {
        [PrimaryKey]
        public Guid? Id { get; set; }
        public Guid? Pres_ID { get; set; }
        public string? Name { get; set; }
        public string? value { get; set; }
        public string? Unit { get; set; }
    }
}
