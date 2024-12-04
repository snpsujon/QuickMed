using QuickMed.ViewModels;

public class PrescriptionViewModel
{
    public PatientData? pdata { get; set; }
    public NextMeetingData? nxtMeetData { get; set; }
    public List<Dictionary<string, string>> ccTableData { get; set; } = new();
    public HealthObservationsData? hoTableData { get; set; }
    public List<Dictionary<string, string>> mhTableData { get; set; } = new();
    public List<Dictionary<string, MeasurementData>> oeTableData { get; set; } = new();
    public List<Dictionary<string, string>> dhTableData { get; set; } = new();
    public List<Dictionary<string, string>> dxTableData { get; set; } = new();
    public List<Dictionary<string, string>> ixTableData { get; set; } = new();
    public List<Dictionary<string, string>> noteTableData { get; set; } = new();
    public List<Dictionary<string, string>> reportTableData { get; set; } = new();
    public List<TreatmentPopVM> treatments { get; set; } = new();
    public List<string> advice { get; set; } = new();
    public string? reffer { get; set; }
    public bool? IsHeader { get; set; } = true;
    public bool? IsPrint { get; set; } = false;
}

public class PatientData
{
    public string? Name { get; set; }
    public string? Age { get; set; }
    public string? Sex { get; set; }
    public string? Address { get; set; }
    public string? Mobile { get; set; }
    public string? RegNo { get; set; }
    public string? Weight { get; set; }
    public string? date { get; set; }
    public string? Height { get; set; }
    public string? BmiWeight { get; set; }
}

public class NextMeetingData
{
    public string? NextMeetingDuration { get; set; }
    public string? NextMeetingDate { get; set; }
    public string? Payment { get; set; }
    public string? ReferredBy { get; set; }
}

public class HealthObservationsData
{
    public Dictionary<string, bool>? HealthCheckData { get; set; } // Nullable or initialize to empty if required
    public string? FreeTextHO { get; set; }
}

public class MeasurementData
{
    public string? Value { get; set; }
    public string? Unit { get; set; }
}
