using SQLite;

namespace QuickMed.DB
{
    public class TblPerceptionTemplate
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
