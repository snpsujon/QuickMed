using SQLite;

namespace QuickMed.DB
{
    public class TblFavouriteDrugTemplate
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? DoseId { get; set; }
        public Guid? InstructionId { get; set; }
        public Guid? DurationId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
