using WGU_App_RileyJuniewic.Data.Models.Enums;

namespace WGU_App_RileyJuniewic.Forms.Components.CourseComponents;

public sealed partial class ModifyCourseCard : ContentView
{
    public static readonly BindableProperty CardTitleProperty =
        BindableProperty.Create(nameof(CardTitle), typeof(string), typeof(ModifyCourseCard), null);

    public string CardTitle
    {
        get => (string)GetValue(CardTitleProperty);
        set => SetValue(CardTitleProperty, value);
    }

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(ModifyCourseCard), null);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty StatusProperty =
        BindableProperty.Create(nameof(Status), typeof(string), typeof(ModifyCourseCard), CourseStatus.InProgress.ToString());

    public string Status
    {
        get => (string)GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    public static readonly BindableProperty StartDateProperty =
        BindableProperty.Create(nameof(StartDate), typeof(DateTime), typeof(ModifyCourseCard), DateTime.Now);

    public DateTime StartDate
    {
        get => (DateTime)GetValue(StartDateProperty);
        set => SetValue(StartDateProperty, value);
    }

    public static readonly BindableProperty EndDateProperty =
        BindableProperty.Create(nameof(EndDate), typeof(DateTime), typeof(ModifyCourseCard), DateTime.Now.AddMonths(1));

    public DateTime EndDate
    {
        get => (DateTime)GetValue(EndDateProperty);
        set => SetValue(EndDateProperty, value);
    }

    public ModifyCourseCard()
    {
        this.InitializeComponent();
    }
}