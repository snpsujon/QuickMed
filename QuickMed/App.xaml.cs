

//using QuickMed.DB;

//namespace QuickMed
//{
//    public partial class App : Application
//    {
//        public App()
//        {
//            InitializeComponent();

//            Database = new ApplicationDbContext();

//            MainPage = new MainPage();

//        }
//        public static ApplicationDbContext Database { get; private set; }
//    }
//}

using QuickMed.DB;
using QuickMed.Services;

namespace QuickMed
{
    public partial class App : Application
    {
        private readonly DataSyncService _dataSyncService;

        public App(ApplicationDbContext dbContext, DataSyncService dataSyncService)
        {
            InitializeComponent();

            Database = dbContext;
            _dataSyncService = dataSyncService;

            // Start the data synchronization service
            _dataSyncService.Start();

            MainPage = new MainPage();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);
            window.Title = "QuickMed";
            return window;
        }

        public static ApplicationDbContext Database { get; private set; }
    }
}
