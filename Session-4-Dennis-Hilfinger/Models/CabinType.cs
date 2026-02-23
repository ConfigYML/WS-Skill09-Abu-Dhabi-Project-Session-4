using System;
using System.Collections.Generic;

namespace Session_4_Dennis_Hilfinger;

public partial class CabinType
{
    public int Id { get; set; }

    public string? CabinName { get; set; }

    public virtual ICollection<SurveyResult> SurveyResults { get; set; } = new List<SurveyResult>();
}
