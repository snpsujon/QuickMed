using SQLite;

namespace QuickMed.DB
{
    public class TblPres_DX
    {
        [PrimaryKey]
        public Guid? Id { get; set; }
        public Guid? Pres_ID { get; set; }
        public Guid? DxTempId { get; set; }
    }
}
