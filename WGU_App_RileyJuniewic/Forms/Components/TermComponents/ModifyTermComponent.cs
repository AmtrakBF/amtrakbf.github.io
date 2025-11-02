namespace WGU_App_RileyJuniewic.Forms.Components.TermComponents;

public sealed partial class ModifyTermComponent : BaseInputContentView
{

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(DateTime), typeof(BaseInputContentView), null);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty StartDateProperty =
        BindableProperty.Create(nameof(StartDate), typeof(DateTime), typeof(BaseInputContentView), DateTime.Now);

    public DateTime StartDate
    {
        get => (DateTime)GetValue(StartDateProperty);
        set => SetValue(StartDateProperty, value);
    }

    public static readonly BindableProperty EndDateProperty =
        BindableProperty.Create(nameof(EndDate), typeof(DateTime), typeof(BaseInputContentView), DateTime.Now.AddMonths(1));

    public DateTime EndDate
    {
        get => (DateTime)GetValue(EndDateProperty);
        set => SetValue(EndDateProperty, value);
    }

    public ModifyTermComponent()
    {
        this.InitializeComponent();
    }
}