namespace Session_4_Dennis_Hilfinger
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ResultsSummaryPage), typeof(ResultsSummaryPage));
            Routing.RegisterRoute(nameof(DetailedResultsPage), typeof(DetailedResultsPage));
        }
    }
}
