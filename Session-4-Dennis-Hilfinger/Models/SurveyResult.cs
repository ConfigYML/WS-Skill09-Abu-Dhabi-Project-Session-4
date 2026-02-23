using System;
using System.Collections.Generic;

namespace Session_4_Dennis_Hilfinger;

public partial class SurveyResult
{
    public int Id { get; set; }

    public int SurveyId { get; set; }

    public int? DepartureAirportId { get; set; }

    public int? ArrivalAirportId { get; set; }

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public int? CabinTypeId { get; set; }

    public short? Question1 { get; set; }

    public short? Question2 { get; set; }

    public short? Question3 { get; set; }

    public short? Question4 { get; set; }

    public virtual Airport? ArrivalAirport { get; set; }

    public virtual CabinType? CabinType { get; set; }

    public virtual Airport? DepartureAirport { get; set; }

    public virtual Survey Survey { get; set; } = null!;
}
