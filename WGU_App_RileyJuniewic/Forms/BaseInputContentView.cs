namespace WGU_App_RileyJuniewic.Forms;

public partial class BaseInputContentView : ContentView
{
    public static readonly BindableProperty CardTitleProperty =
        BindableProperty.Create(nameof(CardTitle), typeof(string), typeof(BaseInputContentView), null);

    public string CardTitle
    {
        get => (string)GetValue(CardTitleProperty);
        set => SetValue(CardTitleProperty, value);
    }

    public static readonly BindableProperty ValidationErrorsProperty =
        BindableProperty.Create(nameof(ValidationErrors), typeof(Dictionary<string, List<string?>>), typeof(BaseInputContentView),new Dictionary<string, List<string?>>());

    public Dictionary<string, List<string?>> ValidationErrors
    {
        get => (Dictionary<string, List<string?>>)GetValue(ValidationErrorsProperty);
        set => SetValue(ValidationErrorsProperty, value);
    }
}