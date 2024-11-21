using SQLite;

namespace QuickMed.DB
{
    public class TblPres_DH
    {
        [PrimaryKey]
        public Guid? Id { get; set; }
        public Guid? Pres_ID { get; set; }
        public Guid? BrandID { get; set; }
    }
}
