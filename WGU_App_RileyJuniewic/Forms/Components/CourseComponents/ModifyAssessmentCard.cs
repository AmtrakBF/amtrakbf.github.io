using WGU_App_RileyJuniewic.Data.Models.Enums;

namespace WGU_App_RileyJuniewic.Forms.Components.CourseComponents;

public sealed partial class ModifyAssessmentCard : ContentView
{
    public static readonly BindableProperty CardTitleProperty =
        BindableProperty.Create(nameof(CardTitle), typeof(string), typeof(ModifyAssessmentCard), null);

    public string CardTitle
    {
        get => (string)GetValue(CardTitleProperty);
        set => SetValue(CardTitleProperty, value);
    }

    public static readonly BindableProperty NameProperty =
        BindableProperty.Create(nameof(Name), typeof(string), typeof(ModifyAssessmentCard), null);

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public static readonly BindableProperty TypeProperty =
        BindableProperty.Create(nameof(Type), typeof(string), typeof(ModifyAssessmentCard), CourseStatus.InProgress.ToString());

    public string Type
    {
        get => (string)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    public static readonly BindableProperty StartDateProperty =
        BindableProperty.Create(nameof(StartDate), typeof(DateTime), typeof(ModifyAssessmentCard), DateTime.Now);

    public DateTime StartDate
    {
        get => (DateTime)GetValue(StartDateProperty);
        set => SetValue(StartDateProperty, value);
    }

    public static readonly BindableProperty EndDateProperty =
        BindableProperty.Create(nameof(EndDate), typeof(DateTime), typeof(ModifyAssessmentCard), DateTime.Now.AddMonths(1));

    public DateTime EndDate
    {
        get => (DateTime)GetValue(EndDateProperty);
        set => SetValue(EndDateProperty, value);
    }

    public ModifyAssessmentCard()
    {
        this.InitializeComponent();
    }
}