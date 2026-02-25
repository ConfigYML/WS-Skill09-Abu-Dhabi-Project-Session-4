using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using System.Linq;

namespace Session_4_Dennis_Hilfinger;

public partial class DetailedResultsPage : ContentPage
{

    public ObservableCollection<ResultDTO> Question1_Results = new ObservableCollection<ResultDTO>();
    public ObservableCollection<ResultDTO> Question2_Results = new ObservableCollection<ResultDTO>();
    public ObservableCollection<ResultDTO> Question3_Results = new ObservableCollection<ResultDTO>();
    public ObservableCollection<ResultDTO> Question4_Results = new ObservableCollection<ResultDTO>();
    public DetailedResultsPage()
	{
		InitializeComponent();
        FillPicker();
        Question1View.ItemsSource = Question1_Results;
        Question2View.ItemsSource = Question2_Results;
        Question3View.ItemsSource = Question3_Results;
        Question4View.ItemsSource = Question4_Results;
    }
    private async void ResultsSummary(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void FillPicker()
    {
        GenderPicker.Items.Add("All genders");
        GenderPicker.Items.Add("M");
        GenderPicker.Items.Add("F");
        GenderPicker.SelectedIndex = 0;

        AgePicker.Items.Add("All ages");
        AgePicker.Items.Add("18-24");
        AgePicker.Items.Add("25-39");
        AgePicker.Items.Add("40-59");
        AgePicker.Items.Add("60+");
        AgePicker.SelectedIndex = 0;

        using (var db = new AirlineContext())
        {
            foreach(var surv in db.Surveys)
            {
                SurveyPicker.Items.Add($"{surv.Month} - {surv.Year}");
            }
        }
    }

    private async void LoadData(object sender, EventArgs e)
    {
        var selectedItem = SurveyPicker.SelectedItem;
        if (selectedItem == null)
        {
            return;
        }
        using (var db = new AirlineContext())
        {
            var split = selectedItem.ToString().Split(" - ");
            short month = short.Parse(split[0]);
            int year = int.Parse(split[1]);
            var surv = db.Surveys.FirstOrDefault(s => s.Month == month && s.Year == year);
            if (surv == null)
            {
                await DisplayAlert("Error", "Selected survey could not be found in data. Please try again later.", "Ok");
                return;
            }

            var economy = await db.CabinTypes.FirstOrDefaultAsync(ct => ct.CabinName == "Economy");
            var business = await db.CabinTypes.FirstOrDefaultAsync(ct => ct.CabinName == "Business");
            var firstClass = await db.CabinTypes.FirstOrDefaultAsync(ct => ct.CabinName == "First");

            var auhDestination = await db.Airports.FirstOrDefaultAsync(a => a.IataCode == "AUH");
            var bahDestination = await db.Airports.FirstOrDefaultAsync(a => a.IataCode == "BAH");
            var dohDestination = await db.Airports.FirstOrDefaultAsync(a => a.IataCode == "DOH");
            var ruhDestination = await db.Airports.FirstOrDefaultAsync(a => a.IataCode == "RUH");
            var caiDestination = await db.Airports.FirstOrDefaultAsync(a => a.IataCode == "CAI");

            string genderFilter = String.Empty;
            string ageFilter = String.Empty;

            if (GenderCheckBox.IsChecked == true)
            {
                var selectedGender = GenderPicker.SelectedItem;
                if (selectedGender != null)
                {
                    if (selectedGender.ToString() == "M" || selectedGender.ToString() == "F")
                    {
                        genderFilter = selectedGender.ToString();
                    }
                }
            }
            if (AgeCheckBox.IsChecked == true)
            {
                var selectedCategory = AgePicker.SelectedItem;
                if (selectedCategory != null)
                {
                    if (selectedCategory.ToString() == "18-24" 
                        || selectedCategory.ToString() == "25-39"
                        || selectedCategory.ToString() == "40-59"
                        || selectedCategory.ToString() == "60+")
                    {
                        ageFilter = selectedCategory.ToString();
                    }
                }
            }


            List<MarkingDTO> question1_markings = new List<MarkingDTO>();
            List<MarkingDTO> question2_markings = new List<MarkingDTO>();
            List<MarkingDTO> question3_markings = new List<MarkingDTO> ();
            List<MarkingDTO> question4_markings = new List<MarkingDTO>();
            var question1_Results = db.SurveyResults.Where(sr => sr.Question1 != 0 && sr.SurveyId == surv.Id);
            var question2_Results = db.SurveyResults.Where(sr => sr.Question2 != 0 && sr.SurveyId == surv.Id);
            var question3_Results = db.SurveyResults.Where(sr => sr.Question3 != 0 && sr.SurveyId == surv.Id);
            var question4_Results = db.SurveyResults.Where(sr => sr.Question4 != 0 && sr.SurveyId == surv.Id);

            bool showMale = true;
            bool showFemale = true;
            bool showEighteen = true;
            bool showTwentyfive = true;
            bool showFourty = true;
            bool showSixty = true;

            if (genderFilter == "M")
            {
                question1_Results = question1_Results.Where(r => r.Gender == "M");
                question2_Results = question2_Results.Where(r => r.Gender == "M");
                question3_Results = question3_Results.Where(r => r.Gender == "M");
                question4_Results = question4_Results.Where(r => r.Gender == "M");
                showFemale = false;
            }
            if (genderFilter == "F")
            {
                question1_Results = question1_Results.Where(r => r.Gender == "F");
                question2_Results = question2_Results.Where(r => r.Gender == "F");
                question3_Results = question3_Results.Where(r => r.Gender == "F");
                question4_Results = question4_Results.Where(r => r.Gender == "F");
                showMale = false;
            }

            if (ageFilter != String.Empty)
            {
                switch (ageFilter)
                {
                    case "18-24":
                        question1_Results = question1_Results.Where(r => r.Age >= 18 && r.Age <= 24);
                        question2_Results = question2_Results.Where(r => r.Age >= 18 && r.Age <= 24);
                        question3_Results = question3_Results.Where(r => r.Age >= 18 && r.Age <= 24);
                        question4_Results = question4_Results.Where(r => r.Age >= 18 && r.Age <= 24);

                        showTwentyfive = false;
                        showFourty = false;
                        showSixty = false;
                        break;
                    case "25-39":
                        question1_Results = question1_Results.Where(r => r.Age >= 25 && r.Age <= 39);
                        question2_Results = question2_Results.Where(r => r.Age >= 25 && r.Age <= 39);
                        question3_Results = question3_Results.Where(r => r.Age >= 25 && r.Age <= 39);
                        question4_Results = question4_Results.Where(r => r.Age >= 25 && r.Age <= 39);

                        showEighteen = false;
                        showFourty = false;
                        showSixty = false;
                        break;
                    case "40-59":
                        question1_Results = question1_Results.Where(r => r.Age >= 40 && r.Age <= 59);
                        question2_Results = question2_Results.Where(r => r.Age >= 40 && r.Age <= 59);
                        question3_Results = question3_Results.Where(r => r.Age >= 40 && r.Age <= 59);
                        question4_Results = question4_Results.Where(r => r.Age >= 40 && r.Age <= 59);

                        showEighteen = false;
                        showTwentyfive = false;
                        showSixty = false;
                        break;
                    case "60+":
                        question1_Results = question1_Results.Where(r => r.Age >= 60);
                        question2_Results = question2_Results.Where(r => r.Age >= 60);
                        question3_Results = question3_Results.Where(r => r.Age >= 60);
                        question4_Results = question4_Results.Where(r => r.Age >= 60);

                        showEighteen = false;
                        showTwentyfive = false;
                        showFourty = false;
                        break;
                }
                
            }

            int question1_TotalCount = question1_Results.Count();
            int question2_TotalCount = question2_Results.Count();
            int question3_TotalCount = question3_Results.Count();
            int question4_TotalCount = question4_Results.Count();

            // Question 1
            question1_markings.Add(new MarkingDTO
            {
                MarkingName = "Outstanding",
                result = question1_Results.Where(r => r.Question1 == 1)
            });
            question1_markings.Add(new MarkingDTO
            {
                MarkingName = "VeryGood",
                result = question1_Results.Where(r => r.Question1 == 2)
            });
            question1_markings.Add(new MarkingDTO
            {
                MarkingName = "Good",
                result = question1_Results.Where(r => r.Question1 == 3)
            });
            question1_markings.Add(new MarkingDTO
            {
                MarkingName = "Adeq",
                result = question1_Results.Where(r => r.Question1 == 4)
            });
            question1_markings.Add(new MarkingDTO
            {
                MarkingName = "Improve",
                result = question1_Results.Where(r => r.Question1 == 5)
            });
            question1_markings.Add(new MarkingDTO
            {
                MarkingName = "Poor",
                result = question1_Results.Where(r => r.Question1 == 6)
            });
            question1_markings.Add(new MarkingDTO
            {
                MarkingName = "Dont",
                result = question1_Results.Where(r => r.Question1 == 7)
            });


            // Question 2
            question2_markings.Add(new MarkingDTO
            {
                MarkingName = "Outstanding",
                result = question2_Results.Where(r => r.Question2 == 1)
            });
            question2_markings.Add(new MarkingDTO
            {
                MarkingName = "VeryGood",
                result = question2_Results.Where(r => r.Question2 == 2)
            });
            question2_markings.Add(new MarkingDTO
            {
                MarkingName = "Good",
                result = question2_Results.Where(r => r.Question2 == 3)
            });
            question2_markings.Add(new MarkingDTO
            {
                MarkingName = "Adeq",
                result = question2_Results.Where(r => r.Question2 == 4)
            });
            question2_markings.Add(new MarkingDTO
            {
                MarkingName = "Improve",
                result = question2_Results.Where(r => r.Question2 == 5)
            });
            question2_markings.Add(new MarkingDTO
            {
                MarkingName = "Poor",
                result = question2_Results.Where(r => r.Question2 == 6)
            });
            question2_markings.Add(new MarkingDTO
            {
                MarkingName = "Dont",
                result = question2_Results.Where(r => r.Question2 == 7)
            });

            
            // Question 3
            question3_markings.Add(new MarkingDTO
            {
                MarkingName = "Outstanding",
                result = question3_Results.Where(r => r.Question3 == 1)
            });
            question3_markings.Add(new MarkingDTO
            {
                MarkingName = "VeryGood",
                result = question3_Results.Where(r => r.Question3 == 2)
            });
            question3_markings.Add(new MarkingDTO
            {
                MarkingName = "Good",
                result = question3_Results.Where(r => r.Question3 == 3)
            });
            question3_markings.Add(new MarkingDTO
            {
                MarkingName = "Adeq",
                result = question3_Results.Where(r => r.Question3 == 4)
            });
            question3_markings.Add(new MarkingDTO
            {
                MarkingName = "Improve",
                result = question3_Results.Where(r => r.Question3 == 5)
            });
            question3_markings.Add(new MarkingDTO
            {
                MarkingName = "Poor",
                result = question3_Results.Where(r => r.Question3 == 6)
            });
            question3_markings.Add(new MarkingDTO
            {
                MarkingName = "Dont",
                result = question3_Results.Where(r => r.Question3 == 7)
            });



            // Question 4
            question4_markings.Add(new MarkingDTO
            {
                MarkingName = "Outstanding",
                result = question4_Results.Where(r => r.Question4 == 1)
            });
            question4_markings.Add(new MarkingDTO
            {
                MarkingName = "VeryGood",
                result = question4_Results.Where(r => r.Question4 == 2)
            });
            question4_markings.Add(new MarkingDTO
            {
                MarkingName = "Good",
                result = question4_Results.Where(r => r.Question4 == 3)
            });
            question4_markings.Add(new MarkingDTO
            {
                MarkingName = "Adeq",
                result = question4_Results.Where(r => r.Question4 == 4)
            });
            question4_markings.Add(new MarkingDTO
            {
                MarkingName = "Improve",
                result = question4_Results.Where(r => r.Question4 == 5)
            });
            question4_markings.Add(new MarkingDTO
            {
                MarkingName = "Poor",
                result = question4_Results.Where(r => r.Question4 == 6)
            });
            question4_markings.Add(new MarkingDTO
            {
                MarkingName = "Dont",
                result = question4_Results.Where(r => r.Question4 == 7)
            });

            bool diffBack = false;
            Question1_Results.Clear();
            Question2_Results.Clear();
            Question3_Results.Clear();
            Question4_Results.Clear();

            foreach (var data in question1_markings)
            {

                var marking = data.result;
                int totalCount = marking.Count();
                int maleCount = marking.Where(m => m.Gender == "M").Count();
                int femaleCount = marking.Where(m => m.Gender == "F").Count();
                int ageCategoryEighteenCount = marking.Where(m => m.Age >= 18 && m.Age <= 24).Count();
                int ageCategoryTwentyfiveCount = marking.Where(m => m.Age >= 25 && m.Age <= 39).Count();
                int ageCategoryFourtyCount = marking.Where(m => m.Age >= 40 && m.Age <= 59).Count();
                int ageCategorySixtyCount = marking.Where(m => m.Age >= 60).Count();
                int economyCount = marking.Where(m => m.CabinTypeId == economy.Id).Count();
                int businessCount = marking.Where(m => m.CabinTypeId == business.Id).Count();
                int firstClassCount = marking.Where(m => m.CabinTypeId == firstClass.Id).Count();
                int auhCount = marking.Where(m => m.ArrivalAirportId == auhDestination.Id).Count();
                int bahCount = marking.Where(m => m.ArrivalAirportId == bahDestination.Id).Count();
                int dohCount = marking.Where(m => m.ArrivalAirportId == dohDestination.Id).Count();
                int ruhCount = marking.Where(m => m.ArrivalAirportId == ruhDestination.Id).Count();
                int caiCount = marking.Where(m => m.ArrivalAirportId == caiDestination.Id).Count();

                Question1_Results.Add(new ResultDTO
                {
                    TotalCount = totalCount.ToString(),
                    MaleCount = maleCount.ToString(),
                    ShowMale = showMale,
                    FemaleCount = femaleCount.ToString(),
                    ShowFemale = showFemale,
                    AgeCategoryEighteenCount = ageCategoryEighteenCount.ToString(),
                    ShowEighteen = showEighteen,
                    AgeCategoryTwentyfiveCount = ageCategoryTwentyfiveCount.ToString(),
                    ShowTwentyfive = showTwentyfive,
                    AgeCategoryFourtyCount = ageCategoryFourtyCount.ToString(),
                    ShowFourty = showFourty,
                    AgeCategorySixtyCount = ageCategorySixtyCount.ToString(),
                    ShowSixty = showSixty,
                    EconomyCount = economyCount.ToString(),
                    BusinessCount = businessCount.ToString(),
                    FirstClassCount = firstClassCount.ToString(),
                    AuhCount = auhCount.ToString(),
                    BahCount = bahCount.ToString(),
                    DohCount = dohCount.ToString(),
                    RuhCount = ruhCount.ToString(),
                    CaiCount = caiCount.ToString(),
                    DifferentBackground = diffBack
                });

                double percentage = double.Parse(totalCount.ToString()) / double.Parse(question1_TotalCount.ToString());
                double maxWidth = Question1VisualizationLayout.Width;
                Rectangle element = FindByName($"Question1{data.MarkingName}Rect") as Rectangle;
                if (element != null)
                {
                    element.WidthRequest = maxWidth * percentage;
                }

                diffBack = !diffBack;
            }

            

            diffBack = false;

            foreach (var data in question2_markings)
            {
                var marking = data.result;
                int totalCount = marking.Count();
                int maleCount = marking.Where(m => m.Gender == "M").Count();
                int femaleCount = marking.Where(m => m.Gender == "F").Count();
                int ageCategoryEighteenCount = marking.Where(m => m.Age >= 18 && m.Age <= 24).Count();
                int ageCategoryTwentyfiveCount = marking.Where(m => m.Age >= 25 && m.Age <= 39).Count();
                int ageCategoryFourtyCount = marking.Where(m => m.Age >= 40 && m.Age <= 59).Count();
                int ageCategorySixtyCount = marking.Where(m => m.Age >= 60).Count();
                int economyCount = marking.Where(m => m.CabinTypeId == economy.Id).Count();
                int businessCount = marking.Where(m => m.CabinTypeId == business.Id).Count();
                int firstClassCount = marking.Where(m => m.CabinTypeId == firstClass.Id).Count();
                int auhCount = marking.Where(m => m.ArrivalAirportId == auhDestination.Id).Count();
                int bahCount = marking.Where(m => m.ArrivalAirportId == bahDestination.Id).Count();
                int dohCount = marking.Where(m => m.ArrivalAirportId == dohDestination.Id).Count();
                int ruhCount = marking.Where(m => m.ArrivalAirportId == ruhDestination.Id).Count();
                int caiCount = marking.Where(m => m.ArrivalAirportId == caiDestination.Id).Count();

                Question2_Results.Add(new ResultDTO
                {
                    TotalCount = totalCount.ToString(),
                    MaleCount = maleCount.ToString(),
                    ShowMale = showMale,
                    FemaleCount = femaleCount.ToString(),
                    ShowFemale = showFemale,
                    AgeCategoryEighteenCount = ageCategoryEighteenCount.ToString(),
                    ShowEighteen = showEighteen,
                    AgeCategoryTwentyfiveCount = ageCategoryTwentyfiveCount.ToString(),
                    ShowTwentyfive = showTwentyfive,
                    AgeCategoryFourtyCount = ageCategoryFourtyCount.ToString(),
                    ShowFourty = showFourty,
                    AgeCategorySixtyCount = ageCategorySixtyCount.ToString(),
                    ShowSixty = showSixty,
                    EconomyCount = economyCount.ToString(),
                    BusinessCount = businessCount.ToString(),
                    FirstClassCount = firstClassCount.ToString(),
                    AuhCount = auhCount.ToString(),
                    BahCount = bahCount.ToString(),
                    DohCount = dohCount.ToString(),
                    RuhCount = ruhCount.ToString(),
                    CaiCount = caiCount.ToString(),
                    DifferentBackground = diffBack
                });

                double percentage = double.Parse(totalCount.ToString()) / double.Parse(question2_TotalCount.ToString());
                double maxWidth = Question2VisualizationLayout.Width;
                Rectangle element = FindByName($"Question2{data.MarkingName}Rect") as Rectangle;
                if (element != null)
                {
                    var elementWidth = maxWidth * percentage;
                    element.WidthRequest = elementWidth;
                }

                diffBack = !diffBack;
            }


            diffBack = false;

            foreach (var data in question3_markings)
            {
                var marking = data.result;
                int totalCount = marking.Count();
                int maleCount = marking.Where(m => m.Gender == "M").Count();
                int femaleCount = marking.Where(m => m.Gender == "F").Count();
                int ageCategoryEighteenCount = marking.Where(m => m.Age >= 18 && m.Age <= 24).Count();
                int ageCategoryTwentyfiveCount = marking.Where(m => m.Age >= 25 && m.Age <= 39).Count();
                int ageCategoryFourtyCount = marking.Where(m => m.Age >= 40 && m.Age <= 59).Count();
                int ageCategorySixtyCount = marking.Where(m => m.Age >= 60).Count();
                int economyCount = marking.Where(m => m.CabinTypeId == economy.Id).Count();
                int businessCount = marking.Where(m => m.CabinTypeId == business.Id).Count();
                int firstClassCount = marking.Where(m => m.CabinTypeId == firstClass.Id).Count();
                int auhCount = marking.Where(m => m.ArrivalAirportId == auhDestination.Id).Count();
                int bahCount = marking.Where(m => m.ArrivalAirportId == bahDestination.Id).Count();
                int dohCount = marking.Where(m => m.ArrivalAirportId == dohDestination.Id).Count();
                int ruhCount = marking.Where(m => m.ArrivalAirportId == ruhDestination.Id).Count();
                int caiCount = marking.Where(m => m.ArrivalAirportId == caiDestination.Id).Count();

                Question3_Results.Add(new ResultDTO
                {
                    TotalCount = totalCount.ToString(),
                    MaleCount = maleCount.ToString(),
                    ShowMale = showMale,
                    FemaleCount = femaleCount.ToString(),
                    ShowFemale = showFemale,
                    AgeCategoryEighteenCount = ageCategoryEighteenCount.ToString(),
                    ShowEighteen = showEighteen,
                    AgeCategoryTwentyfiveCount = ageCategoryTwentyfiveCount.ToString(),
                    ShowTwentyfive = showTwentyfive,
                    AgeCategoryFourtyCount = ageCategoryFourtyCount.ToString(),
                    ShowFourty = showFourty,
                    AgeCategorySixtyCount = ageCategorySixtyCount.ToString(),
                    ShowSixty = showSixty,
                    EconomyCount = economyCount.ToString(),
                    BusinessCount = businessCount.ToString(),
                    FirstClassCount = firstClassCount.ToString(),
                    AuhCount = auhCount.ToString(),
                    BahCount = bahCount.ToString(),
                    DohCount = dohCount.ToString(),
                    RuhCount = ruhCount.ToString(),
                    CaiCount = caiCount.ToString(),
                    DifferentBackground = diffBack
                });
                double percentage = double.Parse(totalCount.ToString()) / double.Parse(question3_TotalCount.ToString());
                double maxWidth = Question3VisualizationLayout.Width;
                Rectangle element = FindByName($"Question3{data.MarkingName}Rect") as Rectangle;
                if (element != null)
                {
                    element.WidthRequest = maxWidth * percentage;
                }
                diffBack = !diffBack;
            }


            diffBack = false;

            foreach (var data in question4_markings)
            {
                var marking = data.result;
                int totalCount = marking.Count();
                int maleCount = marking.Where(m => m.Gender == "M").Count();
                int femaleCount = marking.Where(m => m.Gender == "F").Count();
                int ageCategoryEighteenCount = marking.Where(m => m.Age >= 18 && m.Age <= 24).Count();
                int ageCategoryTwentyfiveCount = marking.Where(m => m.Age >= 25 && m.Age <= 39).Count();
                int ageCategoryFourtyCount = marking.Where(m => m.Age >= 40 && m.Age <= 59).Count();
                int ageCategorySixtyCount = marking.Where(m => m.Age >= 60).Count();
                int economyCount = marking.Where(m => m.CabinTypeId == economy.Id).Count();
                int businessCount = marking.Where(m => m.CabinTypeId == business.Id).Count();
                int firstClassCount = marking.Where(m => m.CabinTypeId == firstClass.Id).Count();
                int auhCount = marking.Where(m => m.ArrivalAirportId == auhDestination.Id).Count();
                int bahCount = marking.Where(m => m.ArrivalAirportId == bahDestination.Id).Count();
                int dohCount = marking.Where(m => m.ArrivalAirportId == dohDestination.Id).Count();
                int ruhCount = marking.Where(m => m.ArrivalAirportId == ruhDestination.Id).Count();
                int caiCount = marking.Where(m => m.ArrivalAirportId == caiDestination.Id).Count();

                Question4_Results.Add(new ResultDTO
                {
                    TotalCount = totalCount.ToString(),
                    MaleCount = maleCount.ToString(),
                    ShowMale = showMale,
                    FemaleCount = femaleCount.ToString(),
                    ShowFemale = showFemale,
                    AgeCategoryEighteenCount = ageCategoryEighteenCount.ToString(),
                    ShowEighteen = showEighteen,
                    AgeCategoryTwentyfiveCount = ageCategoryTwentyfiveCount.ToString(),
                    ShowTwentyfive = showTwentyfive,
                    AgeCategoryFourtyCount = ageCategoryFourtyCount.ToString(),
                    ShowFourty = showFourty,
                    AgeCategorySixtyCount = ageCategorySixtyCount.ToString(),
                    ShowSixty = showSixty,
                    EconomyCount = economyCount.ToString(),
                    BusinessCount = businessCount.ToString(),
                    FirstClassCount = firstClassCount.ToString(),
                    AuhCount = auhCount.ToString(),
                    BahCount = bahCount.ToString(),
                    DohCount = dohCount.ToString(),
                    RuhCount = ruhCount.ToString(),
                    CaiCount = caiCount.ToString(),
                    DifferentBackground = diffBack
                }); 
                double percentage = double.Parse(totalCount.ToString()) / double.Parse(question4_TotalCount.ToString());
                double maxWidth = Question4VisualizationLayout.Width;
                Rectangle element = FindByName($"Question4{data.MarkingName}Rect") as Rectangle;
                if (element != null)
                {
                    element.WidthRequest = maxWidth * percentage;
                }
                diffBack = !diffBack;
            }
        }
    }

    private async void Exit(object sender, EventArgs e)
    {
        Application.Current.Quit();
    }

    public class MarkingDTO
    {
        public string MarkingName { get; set; }
        public IQueryable<SurveyResult> result { get; set; }
    }

    public class ResultDTO
    {
        public int QuestionNumber { get; set; }
        public string TotalCount { get; set; }
        public string MaleCount { get; set; }
        public bool ShowMale { get; set; } = true;
        public string FemaleCount { get; set; }
        public bool ShowFemale { get; set; } = true;
        public string AgeCategoryEighteenCount { get; set; }
        public bool ShowEighteen { get; set; } = true;
        public string AgeCategoryTwentyfiveCount { get; set; }
        public bool ShowTwentyfive { get; set; } = true;
        public string AgeCategoryFourtyCount { get; set; }
        public bool ShowFourty { get; set; } = true;
        public string AgeCategorySixtyCount { get; set; }
        public bool ShowSixty { get; set; } = true;
        public string EconomyCount { get; set; }
        public string BusinessCount { get; set; }
        public string FirstClassCount { get; set; }
        public string AuhCount { get; set; }
        public string BahCount { get; set; }
        public string DohCount { get; set; }
        public string RuhCount { get; set; }
        public string CaiCount { get; set; }
        public bool DifferentBackground { get; set; } = false;

    }
}