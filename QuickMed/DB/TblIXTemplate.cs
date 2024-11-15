using SQLite;

namespace QuickMed.DB
{
    public class TblIXTemplate
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? TemplateName { get; set; }
    }
}
