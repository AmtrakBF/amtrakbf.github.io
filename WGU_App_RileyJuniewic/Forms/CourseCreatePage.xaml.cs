using System.Collections.ObjectModel;
using WGU_App_RileyJuniewic.Data.Dtos.Assessment;
using WGU_App_RileyJuniewic.Data.Misc.Events;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels.CourseViewModes;

namespace WGU_App_RileyJuniewic.Forms.CourseForms;

public sealed partial class AddCoursePage : ContentPage, IQueryAttributable
{
    private AddCourseViewModel _viewModel;

    private Term _term = new();
    public Term Term
    {
        get => _term;
        set
        {
            _term = value;
            _viewModel.SetTerm(value);
            OnPropertyChanged(nameof(Term));
        }
    }

    public EventHandler ButtonEvent { get; set; }

    public AddCoursePage()
    {
        _viewModel = ServiceHelper.GetService<AddCourseViewModel>();
        BindingContext = _viewModel;

        ButtonEvent = OnAddAssessmentClicked;

        _viewModel.OnCreate += (sender, args) =>
        {
            Shell.Current.GoToAsync("..", true);
        };

        InitializeComponent();
    }

    private void OnAddAssessmentClicked(object? sender, EventArgs e)
    {
        var request = (sender as Button)?.BindingContext as CreateAssessmentRequest;
        if (request != null)
        {
            _viewModel.Assessments.Remove(request);
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Term", out var termValue) && termValue is Term term)
        {
            Term = term;
        }
    }
}