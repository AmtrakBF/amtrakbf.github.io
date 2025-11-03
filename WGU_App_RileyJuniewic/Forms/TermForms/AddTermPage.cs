using WGU_App_RileyJuniewic.Data.Misc.Events;

namespace WGU_App_RileyJuniewic.Forms.TermForms;

public partial class AddTermPage : ContentPage
{
    private EventHandler? _onSubmitEvent;
    public EventHandler? OnSubmitEvent
    {
        get => _onSubmitEvent;
        set
        {
            _onSubmitEvent = value;
            OnPropertyChanged(nameof(OnSubmitEvent));
        }
    }

    public AddTermPage()
    {
        InitializeComponent();
        OnSubmitEvent += OnSubmitEventHandler;
    }
    
    private void OnSubmitEventHandler(object? sender, EventArgs e)
    {
        _ = Shell.Current.GoToAsync("..", true);
    }
}