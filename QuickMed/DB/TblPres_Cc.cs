using SQLite;

namespace QuickMed.DB
{
    public class TblPres_Cc
    {
        [PrimaryKey]
        public Guid? Id { get; set; }
        public Guid? Pres_ID { get; set; }
        public string? CcName { get; set; }
        public Guid? DurationId { get; set; }
        public Guid? Dm_Id { get; set; }
    }
}
