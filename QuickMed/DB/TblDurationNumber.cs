using SQLite;

namespace QuickMed.DB
{
    public class TblDurationNumber
    {
        [PrimaryKey]
        public Guid? Id { get; set; }
        public string? Name { get; set; }
    }
}
