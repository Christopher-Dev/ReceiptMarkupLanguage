using RmlEditor.Services;

namespace RmlEditor
{
    public partial class MainPage : ContentPage
    {
        private readonly IDatabaseService _databaseService;

        public MainPage(IDatabaseService databaseService)
        {
            InitializeComponent();

            _databaseService = databaseService;

            // Example usage of DatabaseService
            LoadReceipts();

        }

        private void LoadReceipts()
        {
            var receipts = _databaseService.GetAll();

#if DEBUG

            foreach (var receipt in receipts)
            {
                Console.WriteLine($"Project ID: {receipt.Id}, Project Name: {receipt.ProjectName}, Created On: {receipt.CreatedOn}");
            }

#endif

        }
    }
}
