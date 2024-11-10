namespace QuickMed.ViewModels
{
    public class MenuItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public List<MenuItem> SubMenu { get; set; } = new List<MenuItem>();
    }

}
