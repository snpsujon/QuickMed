using QuickMed.ViewModels;

public class PrescriptionViewModel
{
    public PatientData? PData { get; set; }
    public NextMeetingData? NxtMeetData { get; set; }
    public List<Dictionary<string, string>> CCTableData { get; set; } = new();
    public HealthObservationsData? HOTableData { get; set; }
    public List<Dictionary<string, string>> MHTableData { get; set; } = new();
    public List<Dictionary<string, MeasurementData>> OETableData { get; set; } = new();
    public List<Dictionary<string, string>> DHTableData { get; set; } = new();
    public List<Dictionary<string, string>> DXTableData { get; set; } = new();
    public List<Dictionary<string, string>> IXTableData { get; set; } = new();
    public List<Dictionary<string, string>> NoteTableData { get; set; } = new();
    public List<Dictionary<string, string>> ReportTableData { get; set; } = new();
    public List<TreatmentPopVM> Treatments { get; set; } = new();
    public List<string> Advice { get; set; } = new();
    public string? Reffer { get; set; }
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
    public float? Height { get; set; }
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
