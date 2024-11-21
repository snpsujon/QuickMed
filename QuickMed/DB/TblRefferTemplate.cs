using SQLite;

namespace QuickMed.DB
{
    public class TblRefferTemplate
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Details { get; set; }
    }
}
