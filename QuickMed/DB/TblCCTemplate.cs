using SQLite;

namespace QuickMed.DB
{
    public class TblCCTemplate
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
