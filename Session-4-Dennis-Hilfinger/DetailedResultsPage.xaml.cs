namespace Session_4_Dennis_Hilfinger;

public partial class DetailedResultsPage : ContentPage
{
	public DetailedResultsPage()
	{
		InitializeComponent();
	}
    private async void ResultsSummary(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void Exit(object sender, EventArgs e)
    {
        Application.Current.Quit();
    }
}