using SQLite;

namespace QuickMed.DB
{
    public class TblFavouriteDrugTemplate
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
