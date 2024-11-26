namespace QuickMed.DB
{
    public class TblPres_Note
    {
        public Guid? Id { get; set; }
        public Guid? Pres_ID { get; set; }
        public Guid? NoteTempId { get; set; }
    }
    public class TblPres_NoteTemp
    {
        public Guid? Id { get; set; }
        public Guid? Pres_ID { get; set; }
        public Guid? NoteTempId { get; set; }
    }
}
