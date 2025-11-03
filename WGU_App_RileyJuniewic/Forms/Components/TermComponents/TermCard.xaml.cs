
using WGU_App_RileyJuniewic.Data.Misc.Events;

namespace WGU_App_RileyJuniewic.Forms.Components.TermComponents;
public sealed partial class TermCard : ContentView
{

    public static readonly BindableProperty TermIdProperty =
        BindableProperty.Create(nameof(TermId), typeof(Guid), typeof(TermCard), null);
    public Guid TermId
    {
        get => (Guid)GetValue(TermIdProperty);
        set => SetValue(TermIdProperty, value);
    }

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(TermCard), null);
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    public static readonly BindableProperty StartDateProperty =
        BindableProperty.Create(nameof(StartDate), typeof(DateTime), typeof(TermCard), DateTime.Now, propertyChanged: OnDateChanged);
    public DateTime StartDate
    {
        get => (DateTime)GetValue(StartDateProperty);
        set
        {
            SetValue(StartDateProperty, value);
            OnPropertyChanged(nameof(WeeksRemaining));
            OnPropertyChanged(nameof(CourseCompletion));
        }
    }
    
    public static readonly BindableProperty EndDateProperty =
        BindableProperty.Create(nameof(EndDate), typeof(DateTime), typeof(TermCard), DateTime.Now.AddMonths(1), propertyChanged: OnDateChanged);
    public DateTime EndDate
    {
        get => (DateTime)GetValue(EndDateProperty);
        set
        {
            SetValue(EndDateProperty, value);
            OnPropertyChanged(nameof(WeeksRemaining));
            OnPropertyChanged(nameof(CourseCompletion));
        }
    }
    
    public static readonly BindableProperty TotalCoursesFinishedProperty =
        BindableProperty.Create(nameof(TotalCoursesFinished), typeof(int), typeof(TermCard), 0, propertyChanged: OnCourseChanged);
    public int TotalCoursesFinished
    {
        get => (int)GetValue(TotalCoursesFinishedProperty);
        set
        {
            SetValue(TotalCoursesFinishedProperty, value);
            OnPropertyChanged(nameof(CourseCompletion));
        }
    }
    
    public static readonly BindableProperty TotalCoursesProperty =
        BindableProperty.Create(nameof(TotalCourses), typeof(int), typeof(TermCard), 0, propertyChanged: OnCourseChanged);
    public int TotalCourses
    {
        get => (int)GetValue(TotalCoursesProperty);
        set
        {
            SetValue(TotalCoursesProperty, value);
            OnPropertyChanged(nameof(CourseCompletion));
        }
    }

    public static readonly BindableProperty OnModifyEventProperty =
        BindableProperty.Create(nameof(OnModifyEvent), typeof(EventHandler), typeof(BaseInputContentView), null, BindingMode.TwoWay);

    public EventHandler? OnModifyEvent
    {
        get => (EventHandler?)GetValue(OnModifyEventProperty);
        set => SetValue(OnModifyEventProperty, value);
    }
    
    public string CourseCompletion
    {
        get => $"{TotalCoursesFinished}/{TotalCourses} Courses Complete";
    }
    
    public string WeeksRemaining
    {
        get
        {
            if (StartDate <= DateTime.Now && EndDate >= DateTime.Now)
            {
                var weeksRemaining = (EndDate - DateTime.Now).Days / 7;
                return $"{weeksRemaining} weeks remaining";
            }
            return "";
        }
    }
    
    private static void OnDateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var termCard = (TermCard)bindable;
        termCard.OnPropertyChanged(nameof(WeeksRemaining));
    }

    private static void OnCourseChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var termCard = (TermCard)bindable;
        termCard.OnPropertyChanged(nameof(CourseCompletion));
    }
    
    public TermCard()
    {
        this.InitializeComponent();
    }
    
    private void Modify_Term_Clicked(object sender, EventArgs e)
    {
        OnModifyEvent?.Invoke(this, new GuidEventArgs(TermId));
    }
}