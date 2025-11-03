using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels.CourseViewModes;

namespace WGU_App_RileyJuniewic.Forms.CourseForms;

public sealed partial class AddCoursePage : ContentPage, IQueryAttributable
{
    private Term? _term;
    public Term? Term
    {
        get => _term;
        set
        {
            _term = value;
            OnPropertyChanged(nameof(Term));
        }
    }

    public AddCoursePage()
    {
        var viewModel = ServiceHelper.GetService<AddCourseViewModel>();
        BindingContext = viewModel;

        InitializeComponent();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Term", out var termValue) && termValue is Term term)
        {
            Term = term;
        }
    }
}