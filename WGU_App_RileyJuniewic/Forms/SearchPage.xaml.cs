using System.Windows.Input;
using WGU_App_RileyJuniewic.Data.Dtos.Course;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Misc.Commands;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels;
using WGU_App_RileyJuniewic.Forms.AssessmentForms;
using WGU_App_RileyJuniewic.Forms.CourseForms;
using WGU_App_RileyJuniewic.Forms.InstructorForms;

namespace WGU_App_RileyJuniewic.Forms;

public sealed partial class SearchPage : ContentPage
{
    private readonly SearchViewModel _searchViewModel;


    public SearchPage()
    {
        _searchViewModel = ServiceHelper.GetService<SearchViewModel>();
        BindingContext = _searchViewModel;

        InitializeComponent();
    }

    private void ViewCourseEventHandler(object sender, EventArgs e)
    {
        var senderButton = sender as Button;
        var course = senderButton?.BindingContext as Course;
        if (course is null)
        {
            new ToastNotification("Cannot open course");
            return;
        }

        var navigationParameter = new ShellNavigationQueryParameters
        {
            {"Course", course}
        };
        Shell.Current.GoToAsync(nameof(ViewCoursePage), true, navigationParameter);
    }

    private void ViewTermEventHandler(object sender, EventArgs e)
    {
        var senderButton = sender as Button;
        var term = senderButton?.BindingContext as Term;
        if (term is null)
        {
            new ToastNotification("Cannot open term");
            return;
        }
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { "TermId", term.TermId }
        };
        Shell.Current.GoToAsync(nameof(SelectedTermPage), true, navigationParameter);
    }

    private void ViewAssessmentEventHandler(object sender, EventArgs e)
    {
        var senderButton = sender as Button;
        var assessment = senderButton?.BindingContext as Assessment;
        if (assessment is null)
        {
            new ToastNotification("Cannot modify assessment");
            return;
        }
        var navigationParameter = new ShellNavigationQueryParameters
        {
            {"Assessment", assessment}
        };
        Shell.Current.GoToAsync(nameof(ModifyAssessmentPage), true, navigationParameter);
    }

    private void ViewInstructorEventHandler(object sender, EventArgs e)
    {
        var senderButton = sender as Button;
        var instructor = senderButton?.BindingContext as Instructor;
        if (instructor is null)
        {
            new ToastNotification("Cannot modify instructor");
            return;
        }

        var navigationParameter = new ShellNavigationQueryParameters
        {
            {"Instructor", instructor}
        };
        Shell.Current.GoToAsync(nameof(InstructorUpdatePage), true, navigationParameter);
    }
}