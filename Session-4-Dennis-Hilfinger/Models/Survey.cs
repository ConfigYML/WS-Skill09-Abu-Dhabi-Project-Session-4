using System;
using System.Collections.Generic;

namespace Session_4_Dennis_Hilfinger;

public partial class Survey
{
    public int Id { get; set; }

    public short? Month { get; set; }

    public int? Year { get; set; }

    public virtual ICollection<SurveyResult> SurveyResults { get; set; } = new List<SurveyResult>();
}
