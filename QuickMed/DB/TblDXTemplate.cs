using SQLite;

namespace QuickMed.DB
{
    public class TblDXTemplate
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
