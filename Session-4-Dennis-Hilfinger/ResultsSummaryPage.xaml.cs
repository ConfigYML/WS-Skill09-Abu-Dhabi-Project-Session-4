using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Windows.Globalization.DateTimeFormatting;

namespace Session_4_Dennis_Hilfinger;

public partial class ResultsSummaryPage : ContentPage
{
	public ResultsSummaryPage()
	{
		InitializeComponent();
		LoadData(null, null);
	}

	private async void DetailedResults(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("DetailedResultsPage");
	}

	private async void LoadData(object sender, EventArgs e)
	{
        using(var db = new AirlineContext())
		{
			List<SurveyDTO> survs = new List<SurveyDTO>();
            foreach(var survey in db.Surveys)
			{
				survs.Add(new SurveyDTO()
				{
					Id = survey.Id,
					Month = survey.Month.Value,
					Year = survey.Year.Value
				});
			}

            var economy = await db.CabinTypes.FirstOrDefaultAsync(ct => ct.CabinName == "Economy");
            var business = await db.CabinTypes.FirstOrDefaultAsync(ct => ct.CabinName == "Business");
            var firstClass = await db.CabinTypes.FirstOrDefaultAsync(ct => ct.CabinName == "First");

			var auhDestination = await db.Airports.FirstOrDefaultAsync(a => a.IataCode == "AUH");
			var bahDestination = await db.Airports.FirstOrDefaultAsync(a => a.IataCode == "BAH");
			var dohDestination = await db.Airports.FirstOrDefaultAsync(a => a.IataCode == "DOH");
			var ruhDestination = await db.Airports.FirstOrDefaultAsync(a => a.IataCode == "RUH");
			var caiDestination = await db.Airports.FirstOrDefaultAsync(a => a.IataCode == "CAI");

            var starting = survs.Min(s => s.calcDate);
			var ending = survs.Max(s => s.calcDate);
			ending = ending.AddMonths(1);
			ending = ending.AddDays(-1);
            HeadingLabel.Text = $"Fieldwork: {starting.ToString()} - {ending.ToString()}";

			MaleCountLabel.Text = db.SurveyResults.Where(sr => sr.Gender.ToLower() == "m").Count().ToString();
			FemaleCountLabel.Text = db.SurveyResults.Where(sr => sr.Gender.ToLower() == "f").Count().ToString();
			
			AgeCountLabel_18.Text = db.SurveyResults.Where(sr => sr.Age >= 18 && sr.Age <= 24).Count().ToString();
			AgeCountLabel_25.Text = db.SurveyResults.Where(sr => sr.Age >= 25 && sr.Age <= 39).Count().ToString();
			AgeCountLabel_40.Text = db.SurveyResults.Where(sr => sr.Age >= 40 && sr.Age <= 59).Count().ToString();
			AgeCountLabel_60.Text = db.SurveyResults.Where(sr => sr.Age >= 60).Count().ToString();

			EconomyCountLabel.Text = db.SurveyResults.Where(sr => sr.CabinTypeId == economy.Id).Count().ToString();
			BusinessCountLabel.Text = db.SurveyResults.Where(sr => sr.CabinTypeId == business.Id).Count().ToString();
			FirstCountLabel.Text = db.SurveyResults.Where(sr => sr.CabinTypeId == firstClass.Id).Count().ToString();

			AuhCountLabel.Text = db.SurveyResults.Where(sr => sr.ArrivalAirportId == auhDestination.Id).Count().ToString();
			BahCountLabel.Text = db.SurveyResults.Where(sr => sr.ArrivalAirportId == bahDestination.Id).Count().ToString();
			DohCountLabel.Text = db.SurveyResults.Where(sr => sr.ArrivalAirportId == dohDestination.Id).Count().ToString();
			RuhCountLabel.Text = db.SurveyResults.Where(sr => sr.ArrivalAirportId == ruhDestination.Id).Count().ToString();
			CaiCountLabel.Text = db.SurveyResults.Where(sr => sr.ArrivalAirportId == caiDestination.Id).Count().ToString();
        }

    }

    private async void Exit(object sender, EventArgs e)
    {
        Application.Current.Quit();
    }

	public class SurveyDTO
	{
		public int Id { get; set; }
		public short Month { get; set; }
		public int Year { get; set; }
		public DateOnly calcDate => new DateOnly(Year, Month, 1);
	}
}