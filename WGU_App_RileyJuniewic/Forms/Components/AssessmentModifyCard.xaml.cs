using System.Windows.Input;
using WGU_App_RileyJuniewic.Data.Dtos.Assessment;
using WGU_App_RileyJuniewic.Data.Misc.Events;
using WGU_App_RileyJuniewic.Data.Models.Enums;

namespace WGU_App_RileyJuniewic.Forms.Components.AssessmentComponents;

public sealed partial class ModifyAssessmentCard : BaseInputContentView
{
    public static readonly BindableProperty NameProperty =
        BindableProperty.Create(nameof(Name), typeof(string), typeof(ModifyAssessmentCard), null, BindingMode.TwoWay);

    public static readonly BindableProperty TypeProperty =
        BindableProperty.Create(nameof(Type), typeof(string), typeof(ModifyAssessmentCard), AssessmentType.Performance.ToString(), BindingMode.TwoWay);

    public static readonly BindableProperty StartDateProperty =
       BindableProperty.Create(nameof(StartDate), typeof(DateTime), typeof(ModifyAssessmentCard), DateTime.Now, BindingMode.TwoWay);


    public static readonly BindableProperty EndDateProperty =
        BindableProperty.Create(nameof(EndDate), typeof(DateTime), typeof(ModifyAssessmentCard), DateTime.Now.AddDays(1), BindingMode.TwoWay);

    public static readonly BindableProperty ButtonTextProperty =
        BindableProperty.Create(nameof(ButtonText), typeof(string), typeof(ModifyAssessmentCard), null);

    public static readonly BindableProperty ButtonEventProperty =
        BindableProperty.Create(nameof(ButtonEvent), typeof(EventHandler), typeof(ModifyAssessmentCard), null, BindingMode.TwoWay);

    public static readonly BindableProperty ButtonBackgroundColorProperty =
        BindableProperty.Create(nameof(ButtonBackgroundColor), typeof(Color), typeof(ModifyAssessmentCard), Color.FromArgb("#002f51"));

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public string Type
    {
        get => (string)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    public DateTime StartDate
    {
        get => (DateTime)GetValue(StartDateProperty);
        set => SetValue(StartDateProperty, value);
    }

    public DateTime EndDate
    {
        get => (DateTime)GetValue(EndDateProperty);
        set => SetValue(EndDateProperty, value);
    }

    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    public EventHandler? ButtonEvent
    {
        get => (EventHandler)GetValue(ButtonEventProperty);
        set => SetValue(ButtonEventProperty, value);
    }

    public Color ButtonBackgroundColor
    {
        get => (Color)GetValue(ButtonBackgroundColorProperty);
        set => SetValue(ButtonBackgroundColorProperty, value);
    }

    public ModifyAssessmentCard()
    {
        this.InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var request = new CreateAssessmentRequest()
        {
            Name = Name,
            Type = Type,
            StartDate = StartDate,
            EndDate = EndDate
        };
        var args = new CreateAssessmentEventArgs(request);
        ButtonEvent?.Invoke(sender, args);
    }
}