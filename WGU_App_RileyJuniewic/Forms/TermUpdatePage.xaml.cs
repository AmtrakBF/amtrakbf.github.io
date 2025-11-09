using WGU_App_RileyJuniewic.Data.Misc.Events;
using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Forms.TermForms;

public partial class UpdateTermPage : ContentPage, IQueryAttributable
{
    private Term? _term;
    public Term? Term
    {
        get => _term;
        set
        {
            _term = value;
            OnPropertyChanged(nameof(Term));
        }
    }
    
    private EventHandler? _onCloseEvent;
    public EventHandler? OnCloseEvent
    {
        get => _onCloseEvent;
        set
        {
            _onCloseEvent = value;
            OnPropertyChanged(nameof(OnCloseEvent));
        }
    }


    public UpdateTermPage()
    {
        InitializeComponent();

        OnCloseEvent += OnCloseEventHandler;
    }

    private void OnCloseEventHandler(object? sender, EventArgs e) => _ = Shell.Current.GoToAsync("..", true);

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Term", out var termValue) && termValue is Term term)
        {
            Term = term;
        }
    }
}