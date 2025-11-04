using System.Collections.ObjectModel;
using System.ComponentModel;
using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos.Assessment;
using WGU_App_RileyJuniewic.Data.Dtos.Course;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Misc.Commands;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;
using WGU_App_RileyJuniewic.Data.Services;

namespace WGU_App_RileyJuniewic.Data.ViewModels.CourseViewModes;

public class AddCourseViewModel : CreateViewModelBase<CreateCourseRequest, Models.Course>
{
    private readonly IAssessmentService _assessmentService;
    
    private ObservableCollection<CreateAssessmentRequest> _assessments = new();
    public ObservableCollection<CreateAssessmentRequest> Assessments
    {
        get => _assessments;
        set
        {
            _assessments = value;
            OnPropertyChanged(nameof(Assessments));
        }
    }

    
    private Instructor _instructor = new();
    public Instructor Instructor
    {
        get => _instructor;
        set
        {
            _instructor = value;
            if (CreateRequest is not null && value is not null)
                CreateRequest.InstructorId = value.InstructorId;
                
            OnPropertyChanged(nameof(Instructor));
        }
    }

    public override event EventHandler<EventArgs>? OnCreate;
    public OnClickCommand AddAssessmentCommand { get; set; }

    public AddCourseViewModel(
        ICreateService<CreateCourseRequest, Models.Course> createService,
        IAssessmentService assessmentService
    ) : base(createService)
    {
        _assessmentService = assessmentService;

        AddAssessmentCommand = new OnClickCommand(AddAssessment);
    }

    public void SetTerm(Term term)
    {
        CreateRequest.TermId = term.TermId;
    }

    private void AddAssessment()
    {
        Assessments.Add(new CreateAssessmentRequest());
    }

    protected override async Task CreateAsync()
    {
        if (CreateRequest.HasErrors)
        {
            new UserError(errors: CreateRequest.GetAllErrors());
            return;
        }

        foreach (var assessment in Assessments)
        {
            if (assessment.HasErrors)
            {
                new UserError(errors: assessment.GetAllErrors());
                return;
            }
        }

        var requestResult = await _createService.CreateAsync(CreateRequest);
        if (requestResult.IsError())
        {
            new UserError(requestResult.Errors);
            return;
        }

        foreach (var assessment in Assessments)
        {
            assessment.CourseId = requestResult.Value.CourseId;
            var assessmentResult = await _assessmentService.CreateAssessmentAsync(assessment);
            if (assessmentResult.IsError())
            {
                new UserError(assessmentResult.Errors);
            }
        }
        
        OnCreate?.Invoke(this, EventArgs.Empty);
    }
}