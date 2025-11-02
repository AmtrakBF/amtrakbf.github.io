namespace WGU_App_RileyJuniewic.Forms.Components.InstructorComponents;

public sealed partial class ModifyInstructorCard : BaseInputContentView
{
    public static readonly BindableProperty NameProperty =
        BindableProperty.Create(nameof(Name), typeof(string), typeof(ModifyInstructorCard), string.Empty, BindingMode.TwoWay);

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public static readonly BindableProperty PhoneProperty =
        BindableProperty.Create(nameof(Phone), typeof(string), typeof(ModifyInstructorCard), string.Empty, BindingMode.TwoWay);

    public string Phone
    {
        get => (string)GetValue(PhoneProperty);
        set => SetValue(PhoneProperty, value);
    }

    public static readonly BindableProperty EmailProperty =
        BindableProperty.Create(nameof(Email), typeof(string), typeof(ModifyInstructorCard), string.Empty, BindingMode.TwoWay);

    public string Email
    {
        get => (string)GetValue(EmailProperty);
        set => SetValue(EmailProperty, value);
    }

    public ModifyInstructorCard()
    {
        this.InitializeComponent();
    }
}