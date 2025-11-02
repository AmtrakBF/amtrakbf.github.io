using System.ComponentModel;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Dtos.Assessment;
using WGU_App_RileyJuniewic.Data.Dtos.Course;
using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Data.ViewModels.CourseViewModes;

public class AddCourseViewModel : BindingModel
{
    
    private BindingList<CreateAssessmentRequest> _assessments = new();
    public BindingList<CreateAssessmentRequest> Assessments
    {
        get => _assessments;
        set
        {
            _assessments = value;
            OnPropertyChanged(nameof(Assessments));
        }
    }

    private CreateCourseRequest _createCourseRequest = new();
    public CreateCourseRequest CreateCourseRequest
    {
        get => _createCourseRequest;
        set
        {
            _createCourseRequest = value;
            OnPropertyChanged(nameof(CreateCourseRequest));
        }
    }

    private Instructor? _instructor;
    public Instructor? Instructor
    {
        get => _instructor;
        set
        {
            _instructor = value;
            OnPropertyChanged(nameof(Instructor));
        }
    }
}