namespace QuickMed.ViewModels
{
    public class FavouriteDrugTempVM
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? BrandId { get; set; }
        public string? BrandName { get; set; }
        public Guid? DoseId { get; set; }
        public string? DoseName { get; set; }
        public Guid? InstructionId { get; set; }
        public string? InstructionName { get; set; }
        public Guid? DurationId { get; set; }
        public string? DurationName { get; set; }
    }
}
