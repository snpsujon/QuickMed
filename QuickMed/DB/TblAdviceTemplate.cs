using SQLite;

namespace QuickMed.DB
{
    public class TblAdviceTemplate
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? AdviceTemplateName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
