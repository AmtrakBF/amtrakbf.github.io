using Plugin.LocalNotification;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels.CourseViewModes;
using WGU_App_RileyJuniewic.Forms.AssessmentForms;
using WGU_App_RileyJuniewic.Forms.InstructorForms;

namespace WGU_App_RileyJuniewic.Forms.CourseForms;

public sealed partial class ViewCoursePage : ContentPage, IQueryAttributable
{
    private ViewCourseViewModel _viewModel;

    private Course _course = new();
    public Course Course
    {
        get => _course;
        set
        {
            _course = value;
            _ = _viewModel.LoadDataAsync(value);
            OnPropertyChanged(nameof(Course));
        }
    }

    public EventHandler? OnModifyCourseEvent { get; set; }

    public ViewCoursePage()
    {
        _viewModel = ServiceHelper.GetService<ViewCourseViewModel>();
        BindingContext = _viewModel;

        OnModifyCourseEvent += ModifyCourseEventHandler;

        InitializeComponent();
    }

    public void ModifyInstructorEventHandler(object sender, EventArgs e)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { "FullCourse", _viewModel.FullCourse }
        };
        _ = Shell.Current.GoToAsync(nameof(ModifyInstructorPage), true, navigationParameter);
    }

    public void ModifyCourseEventHandler(object? sender, EventArgs e)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { "Course", Course }
        };
        _ = Shell.Current.GoToAsync(nameof(ModifyCoursePage), true, navigationParameter);
    }

    public void AddAssessmentEventHandler(object? sender, EventArgs e)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { "Course", Course }
        };
        _ = Shell.Current.GoToAsync(nameof(CreateAssessmentPage), true, navigationParameter);
    }

    public void ModifyAssessessmentEventHandler(object? sender, EventArgs e)
    {
        var senderButton = sender as Button;
        var assessment = senderButton?.BindingContext as Assessment;
        if (assessment is null)
        {
            new UserError("Cannot modify assessment");
            return;
        }

        var navigationParameter = new ShellNavigationQueryParameters
        {
            { "Assessment", assessment }
        };
        _ = Shell.Current.GoToAsync(nameof(ModifyAssessmentPage), true, navigationParameter);
    }

    public void SetAssessmentReminderEventHandler(object? sender, EventArgs e)
    {
        var senderButton = sender as Button;
        var assessment = senderButton?.BindingContext as Assessment;

        if (assessment is null)
        {
            new UserError("Cannot set assessment alert");
            return;
        }

        _ = ShowAssessmentNotification(assessment);
    }

    public void RemoveAssessmentReminderEventHandler(object sender, EventArgs e)
    {
        var senderButton = sender as Button;
        var assessment = senderButton?.BindingContext as Assessment;

        if (assessment is null)
        {
            new UserError("Cannot set assessment alert");
            return;
        }

        _ = _viewModel.RemoveAssessmentNotificationAsync(assessment.AssessmentId);
    }

    public void SetCourseReminderEventHandler(object sender, EventArgs e)
    {
        _ = ShowCourseNotification();
    }

    public void RemoveCourseReminderEventHandler(object sender, EventArgs e)
    {
        _ = _viewModel.RemoveCourseNotificationAsync(Course.CourseId);
    }

    private async Task ShowAssessmentNotification(Assessment assessment)
    {
        var notificationTime = assessment.StartDate.AddDays(-1);
        if (assessment.StartDate < DateTime.Now)
            notificationTime = DateTime.Now.AddSeconds(1);

        var notificationId1 = await SetStartAndEndNotifcations(
            $"{assessment.Type} Assessment {assessment.Name} Starts Soon",
            $"{assessment.StartDate.Date:MMMM dd, yyyy} - {assessment.EndDate.Date:MMMM dd, yyyy}",
            notificationTime,
            assessment.EndDate
        );

        var notifcationEndDate = _viewModel.FullCourse.Course.EndDate.AddDays(-7);
        if (_viewModel.FullCourse.Course.EndDate.AddDays(-7) < DateTime.Now)
            notifcationEndDate = DateTime.Now.AddSeconds(1);

        var notificationId2 = await SetStartAndEndNotifcations(
            $"{assessment.Type} Assessment {assessment.Name} Ending Soon",
            $"The assessment ends {assessment.EndDate.Date:MMMM dd, yyyy}",
            notifcationEndDate,
            assessment.EndDate
        );
        
        await _viewModel.SetAssessmentNotificationAsync(assessment.AssessmentId, notificationId1, notificationId2);
    }
    
    private async Task ShowCourseNotification()
    {
        var notificationTime = _viewModel.FullCourse.Course.StartDate.AddDays(-1);
        if (_viewModel.FullCourse.Course.StartDate < DateTime.Now)
            notificationTime = DateTime.Now.AddSeconds(1);

        var NotificationId1 = await SetStartAndEndNotifcations(
            $"Upcoming Course: {_viewModel.FullCourse.Course.Title}",
            $"{_viewModel.FullCourse.Course.StartDate.Date:MMMM dd, yyyy} - {_viewModel.FullCourse.Course.EndDate.Date:MMMM dd, yyyy}",
            notificationTime,
            _viewModel.FullCourse.Course.EndDate
        );

        var notifcationEndDate = _viewModel.FullCourse.Course.EndDate.AddDays(-7);
        if (_viewModel.FullCourse.Course.EndDate.AddDays(-7) < DateTime.Now)
            notifcationEndDate = DateTime.Now.AddSeconds(1);

        var notificationId2 = await SetStartAndEndNotifcations(
            $"Course Ending Soon: {_viewModel.FullCourse.Course.Title}",
            $"Course ends {_viewModel.FullCourse.Course.EndDate.Date:MMMM dd, yyyy}",
            notifcationEndDate,
            _viewModel.FullCourse.Course.EndDate
        );
        
        await _viewModel.SetCourseNotificationAsync(_viewModel.FullCourse.Course.CourseId, NotificationId1, notificationId2);
    }

    public void ShareNotesEventHandler(object sender, EventArgs e)
    {
        _ = Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = _viewModel.FullCourse.Course.Notes,
            Title = "Share Notes"
        });
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Course", out var courseValue) && courseValue is Course course)
        {
            Course = course;
        }
    }

    private async Task<int> SetStartAndEndNotifcations(string title, string description, DateTime start, DateTime end)
    {
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }

        var rand = new Random();
        var notificationId = rand.Next(1, 1000000);

        var request = new NotificationRequest
        {
            NotificationId = notificationId,
            Title = title,
            Description = description,
            CategoryType = NotificationCategoryType.Reminder,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = start,
                NotifyAutoCancelTime = end
            }
        };

        await LocalNotificationCenter.Current.Show(request);
        return notificationId;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _viewModel.LoadDataAsync(Course);
    }
}