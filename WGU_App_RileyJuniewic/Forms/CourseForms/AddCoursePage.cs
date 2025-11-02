using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels.CourseViewModesl;

namespace WGU_App_RileyJuniewic.Forms.CourseForms;

[QueryProperty(nameof(Term), "Term")]
public sealed partial class AddCoursePage : ContentPage
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
}