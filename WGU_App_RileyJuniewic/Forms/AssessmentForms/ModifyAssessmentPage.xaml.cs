using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels.AssessmentViewModels;

namespace WGU_App_RileyJuniewic.Forms.AssessmentForms;

public partial class ModifyAssessmentPage : ContentPage, IQueryAttributable
{
    private readonly ModifyAssessmentViewModel _viewModel;

    private Assessment _assessment = new();
    public Assessment Assessment
    {
        get => _assessment;
        set
        {
            _assessment = value;
            _viewModel.SetAssessment(value);
            OnPropertyChanged(nameof(Assessment));
        }
    }

    public ModifyAssessmentPage()
    {
        _viewModel = ServiceHelper.GetService<ModifyAssessmentViewModel>();
        BindingContext = _viewModel;

        _viewModel.OnModify += GoBackEventHandler;
        _viewModel.OnDelete += GoBackEventHandler;

        InitializeComponent();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Assessment", out var assessmentValue) && assessmentValue is Assessment assessment)
        {
            Assessment = assessment;
        }
    }

    private void GoBackEventHandler(object? sender, EventArgs e) =>
        _ = Shell.Current.GoToAsync("..", true);
}