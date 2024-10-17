using RmlEditor.Services;

namespace RmlEditor
{
    public partial class App : Application
    {
        private readonly IDatabaseService _databaseService;

        public App(IDatabaseService databaseService)
        {
            InitializeComponent();

            _databaseService = databaseService;

            // Inject the IDatabaseService into MainPage
            MainPage = new MainPage(_databaseService);
        }
    }
}
