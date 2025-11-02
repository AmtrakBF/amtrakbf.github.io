namespace WGU_App_RileyJuniewic.Forms.Components.TermComponents;

public sealed partial class TermCard : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(TermCard), null);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty StartDateProperty =
        BindableProperty.Create(nameof(StartDate), typeof(DateTime), typeof(TermCard), DateTime.Now);

    public DateTime StartDate
    {
        get => (DateTime)GetValue(StartDateProperty);
        set => SetValue(StartDateProperty, value);
    }

    public static readonly BindableProperty EndDateProperty =
        BindableProperty.Create(nameof(EndDate), typeof(DateTime), typeof(TermCard), DateTime.Now.AddMonths(1));

    public DateTime EndDate
    {
        get => (DateTime)GetValue(EndDateProperty);
        set => SetValue(EndDateProperty, value);
    }

    public static readonly BindableProperty TotalCoursesFinishedProperty =
        BindableProperty.Create(nameof(TotalCoursesFinished), typeof(int), typeof(TermCard), 0);

    public int TotalCoursesFinished
    {
        get => (int)GetValue(TotalCoursesFinishedProperty);
        set => SetValue(TotalCoursesFinishedProperty, value);
    }

     public static readonly BindableProperty TotalCoursesProperty =
        BindableProperty.Create(nameof(TotalCourses), typeof(int), typeof(TermCard), 0);

    public int TotalCourses
    {
        get => (int)GetValue(TotalCoursesProperty);
        set => SetValue(TotalCoursesProperty, value);
    }

    public string CourseCompletion
    {
        get => $"{TotalCoursesFinished}/{TotalCourses} Courses Done";
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

    public TermCard()
    {
        this.InitializeComponent();
    }

    private void Modify_Term_Clicked(object sender, EventArgs e)
    {
        _ = Shell.Current.GoToAsync(nameof(ModifyTermPage));
    }
}