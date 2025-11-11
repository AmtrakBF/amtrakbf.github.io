namespace WGU_App_RileyJuniewic.Data.Dtos;

public class ReportDto : BindingModel
{
    private int _completeCourseCount;
    private int _droppedCourseCount;
    private int _activeCourseCount;
    private int _plannedCourseCount;
    private int _completeAssessmentCount;
    private int _incompleteAssessmentCount;
    private int _totalCourseCount;
    private int _totalAssessmentCount;
    private int _totalTermCount;
    private int _completeTermCount;
    private DateTime _reportDate;

    public int CompleteCourseCount
    {
        get => _completeCourseCount;
        set => SetValue(nameof(CompleteCourseCount), ref _completeCourseCount, value);
    }

    public int DroppedCourseCount
    {
        get => _droppedCourseCount;
        set => SetValue(nameof(DroppedCourseCount), ref _droppedCourseCount, value);
    }

    public int ActiveCourseCount
    {
        get => _activeCourseCount;
        set => SetValue(nameof(ActiveCourseCount), ref _activeCourseCount, value);
    }

    public int PlannedCourseCount
    {
        get => _plannedCourseCount;
        set => SetValue(nameof(PlannedCourseCount), ref _plannedCourseCount, value);
    }

    public int CompleteAssessmentCount
    {
        get => _completeAssessmentCount;
        set => SetValue(nameof(CompleteAssessmentCount), ref _completeAssessmentCount, value);
    }

    public int IncompleteAssessmentCount
    {
        get => _incompleteAssessmentCount;
        set => SetValue(nameof(IncompleteAssessmentCount), ref _incompleteAssessmentCount, value);
    }

    public int TotalCourseCount
    {
        get => _totalCourseCount;
        set => SetValue(nameof(TotalCourseCount), ref _totalCourseCount, value);
    }

    public int TotalAssessmentCount
    {
        get => _totalAssessmentCount;
        set => SetValue(nameof(TotalAssessmentCount), ref _totalAssessmentCount, value);
    }

    public int TotalTermCount
    {
        get => _totalTermCount;
        set => SetValue(nameof(TotalTermCount), ref _totalTermCount, value);
    }

    public int CompleteTermCount
    {
        get => _completeTermCount;
        set => SetValue(nameof(CompleteTermCount), ref _completeTermCount, value);
    }

    public DateTime ReportDate
    {
        get => _reportDate;
        set => SetValue(nameof(ReportDate), ref _reportDate, value);
    }

    public ReportDto()
    {
        ValidateAll<ReportDto>();
    }
}