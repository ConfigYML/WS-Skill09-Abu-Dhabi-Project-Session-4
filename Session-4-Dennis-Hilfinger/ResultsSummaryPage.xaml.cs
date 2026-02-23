namespace Session_4_Dennis_Hilfinger;

public partial class ResultsSummaryPage : ContentPage
{
	public ResultsSummaryPage()
	{
		InitializeComponent();
	}

	private async void DetailedResults(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("DetailedResultsPage");
	}

    private async void Exit(object sender, EventArgs e)
    {
        Application.Current.Quit();
    }
}