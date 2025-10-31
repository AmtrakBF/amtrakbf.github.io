using System.Windows.Input;
using WGU_App_RileyJuniewic.Data.Misc.Events;

namespace WGU_App_RileyJuniewic.Forms.Components.InstructorComponents;

public partial class ModifyInstructorContentView : ContentView
{
    public static readonly BindableProperty OnCancelProperty =
        BindableProperty.Create(nameof(OnCancel), typeof(ICommand), typeof(ModifyInstructorContentView), null, BindingMode.TwoWay);

    public ICommand OnCancel
    {
        get => (ICommand)GetValue(OnCancelProperty);
        set => SetValue(OnCancelProperty, value);
    }

    public static readonly BindableProperty OnSubmitProperty =
        BindableProperty.Create(nameof(OnSubmit), typeof(EventHandler<InstructorEventArgs>), typeof(ModifyInstructorContentView), null, BindingMode.TwoWay);

    public EventHandler<InstructorEventArgs> OnSubmit
    {
        get => (EventHandler<InstructorEventArgs>)GetValue(OnSubmitProperty);
        set => SetValue(OnSubmitProperty, value);
    }
}