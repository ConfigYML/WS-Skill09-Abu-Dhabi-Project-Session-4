using System;
using System.Collections.Generic;

namespace Session_4_Dennis_Hilfinger;

public partial class Airport
{
    public int Id { get; set; }

    public string? IataCode { get; set; }

    public virtual ICollection<SurveyResult> SurveyResultArrivalAirports { get; set; } = new List<SurveyResult>();

    public virtual ICollection<SurveyResult> SurveyResultDepartureAirports { get; set; } = new List<SurveyResult>();
}
