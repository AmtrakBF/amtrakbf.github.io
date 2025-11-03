using System.Windows.Input;
using WGU_App_RileyJuniewic.Data.Misc.Events;

namespace WGU_App_RileyJuniewic.Forms;

public partial class ModifyContentView : ContentView
{
    public static readonly BindableProperty OnCancelProperty =
        BindableProperty.Create(nameof(OnCancel), typeof(EventHandler), typeof(ModifyContentView), null, BindingMode.TwoWay);

    public EventHandler? OnCancel
    {
        get => (EventHandler?)GetValue(OnCancelProperty);
        set => SetValue(OnCancelProperty, value);
    }

    public static readonly BindableProperty OnSubmitProperty =
        BindableProperty.Create(nameof(OnSubmit), typeof(EventHandler), typeof(ModifyContentView), null, BindingMode.TwoWay);

    public EventHandler? OnSubmit
    {
        get => (EventHandler?)GetValue(OnSubmitProperty);
        set => SetValue(OnSubmitProperty, value);
    }
}