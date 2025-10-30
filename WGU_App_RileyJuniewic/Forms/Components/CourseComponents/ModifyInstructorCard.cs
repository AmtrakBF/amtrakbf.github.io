using WGU_App_RileyJuniewic.Data.Models.Enums;

namespace WGU_App_RileyJuniewic.Forms.Components.CourseComponents;

public sealed partial class ModifyInstructorCard : ContentView
{
    public static readonly BindableProperty CardTitleProperty =
        BindableProperty.Create(nameof(CardTitle), typeof(string), typeof(ModifyInstructorCard), null);

    public string CardTitle
    {
        get => (string)GetValue(CardTitleProperty);
        set => SetValue(CardTitleProperty, value);
    }

    public static readonly BindableProperty NameProperty =
        BindableProperty.Create(nameof(Name), typeof(string), typeof(ModifyInstructorCard), string.Empty);

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public static readonly BindableProperty PhoneProperty =
        BindableProperty.Create(nameof(Phone), typeof(string), typeof(ModifyInstructorCard), string.Empty);

    public string Phone
    {
        get => (string)GetValue(PhoneProperty);
        set => SetValue(PhoneProperty, value);
    }

    public static readonly BindableProperty EmailProperty =
        BindableProperty.Create(nameof(Email), typeof(string), typeof(ModifyInstructorCard), string.Empty);

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