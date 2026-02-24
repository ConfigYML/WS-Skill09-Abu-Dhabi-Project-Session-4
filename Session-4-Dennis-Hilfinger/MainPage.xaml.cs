
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace Session_4_Dennis_Hilfinger
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void ResultsSummary(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("ResultsSummaryPage");
        }

        private async void ImportData(object sender, EventArgs e)
        {
            var file = await FilePicker.PickAsync(new PickOptions()
            {
                PickerTitle = "Pick a survey csv file to import"
            });
            if (String.IsNullOrEmpty(file.FileName))
            {
                return;
            }
            if (!File.Exists(file.FullPath))
            {
                await DisplayAlert("Info", "Selected file does not exist", "Ok");
                return;
            }
            if (!file.FullPath.EndsWith(".csv"))
            {
                await DisplayAlert("Info", "Please choose a .csv file", "Ok");
                return;
            }

            try
            {
                var nameParts = file.FileName.Split('_');
                if (nameParts.Length < 5)
                {
                    await DisplayAlert("Info", "File name must be in the following format: WSC[YYYY]_TP09_M4_survey_[MM].csv (square brackets are to be ignored)", "Ok");
                    return;
                }
                if (!int.TryParse(nameParts[0].Split("WSC")[1], out int Year))
                {
                    await DisplayAlert("Info", "Year was not a valid number", "Ok");
                    return;
                }
                if (!short.TryParse(nameParts[4].Substring(0, 2), out short Month))
                {
                    await DisplayAlert("Info", "Month was not a valid number", "Ok");
                    return;
                }
                if (Month < short.Parse(1.ToString()) || Month > short.Parse(12.ToString()))
                {
                    await DisplayAlert("Info", "Month must be a number from 01 to 12", "Ok");
                    return;
                }
                using (var db = new AirlineContext())
                {
                    Survey surv;
                    if (db.Surveys.Any(s => s.Year == Year && s.Month == Month))
                    {
                        var accepted = await DisplayAlert("Info", "Survey data for this month already exists. Do you still want to add your results?", "Yes", "No");
                        if (!accepted)
                        {
                            return;
                        }
                        surv = await db.Surveys.FirstOrDefaultAsync(s => s.Year == Year && s.Month == Month);
                    }
                    else
                    {
                        db.Surveys.Add(new Survey()
                        {
                            Year = Year,
                            Month = Month
                        });
                        await db.SaveChangesAsync();
                        surv = await db.Surveys.FirstOrDefaultAsync(s => s.Year == Year && s.Month == Month);
                    }

                    string[] lines = await File.ReadAllLinesAsync(file.FullPath);
                    bool firstLine = true;

                    foreach (var line in lines)
                    {
                        if (firstLine)
                        {
                            firstLine = false;
                            continue;
                        }
                        var values = line.Split(',');
                        if (values.Length <= 0)
                        {
                            continue;
                        }
                        var departureAirport = db.Airports.FirstOrDefault(a => a.IataCode == values[0]);
                        var arrivalAirport = db.Airports.FirstOrDefault(a => a.IataCode == values[1]);
                        if (!int.TryParse(values[2], out int age))
                        {
                            age = 0;
                        }
                        var gender = values[3];
                        var cabinType = db.CabinTypes.FirstOrDefault(ct => ct.CabinName == values[4]);
                        var question_1 = short.Parse(values[5]);
                        var question_2 = short.Parse(values[6]);
                        var question_3 = short.Parse(values[7]);
                        var question_4 = short.Parse(values[8]);
                        db.SurveyResults.Add(new SurveyResult()
                        {
                            SurveyId = surv.Id,
                            DepartureAirportId = departureAirport == null ? null : departureAirport.Id,
                            ArrivalAirportId = arrivalAirport == null ? null : arrivalAirport.Id,
                            Age = age > 0 ? age : null,
                            Gender = String.IsNullOrEmpty(gender) ? null : gender,
                            CabinTypeId = cabinType == null ? null : cabinType.Id,
                            Question1 = question_1,
                            Question2 = question_2,
                            Question3 = question_3,
                            Question4 = question_4,
                        });
                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                await DisplayAlert("Error", "Something went wrong while processing the data to be imported. Please check if the file you provided isn't being used by another process and if the file is correctly formatted.", "Ok");
            }

        }


    }

}
