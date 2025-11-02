using System.Collections;
using System.ComponentModel;

namespace WGU_App_RileyJuniewic.Forms.Components.InstructorComponents;

public sealed partial class ModifyInstructorCard : ContentView
{
    public static readonly BindableProperty CardTitleProperty =
        BindableProperty.Create(nameof(CardTitle), typeof(string), typeof(ModifyInstructorCard), string.Empty, BindingMode.TwoWay);

    public string CardTitle
    {
        get => (string)GetValue(CardTitleProperty);
        set => SetValue(CardTitleProperty, value);
    }

    public static readonly BindableProperty ValidationErrorsProperty =
        BindableProperty.Create(nameof(ValidationErrors), typeof(Dictionary<string, List<string?>>), typeof(ModifyInstructorCard),
            new Dictionary<string, List<string?>>(), propertyChanged: OnValidationErrorsPropertyChanged);

    public Dictionary<string, List<string?>> ValidationErrors
    {
        get => (Dictionary<string, List<string?>>)GetValue(ValidationErrorsProperty);
        set => SetValue(ValidationErrorsProperty, value);
    }

    private static void OnValidationErrorsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var card = (ModifyInstructorCard)bindable;
        // if (newValue is  Dictionary<string, bool> dictionary)
        //     card.ValidationDictionary = dictionary;
    }

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