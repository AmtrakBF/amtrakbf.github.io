using WGU_App_RileyJuniewic.Data.Misc.Events;
using WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;
using WGU_App_RileyJuniewic.Forms.TermForms;

namespace WGU_App_RileyJuniewic.Forms;

public partial class ViewTermsPage : ContentPage
{
    private readonly ViewTermsViewModel _viewModel;

    private EventHandler? _onModifyEvent;
    public EventHandler? OnModifyEvent
    {
        get => _onModifyEvent;
        set
        {
            _onModifyEvent = value;
            OnPropertyChanged(nameof(OnModifyEvent));
        }
    }

    public ViewTermsPage(ViewTermsViewModel viewModel)
    {
        BindingContext = viewModel;
        _viewModel = viewModel;
        InitializeComponent();

        OnModifyEvent += Modify_Term_Clicked;
    }

     protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadDataCommand.Execute(null);
    }

    private void Add_Term_Clicked(object sender, EventArgs e)
    {
        _ = Shell.Current.GoToAsync(nameof(AddTermPage));
    }

    private void Modify_Term_Clicked(object? sender, EventArgs e)
    {
        var termId = e as GuidEventArgs;
        if (termId is null) return;

        var term = _viewModel.Terms.FirstOrDefault(term => term.TermId == termId.Value);
        if (term is null) return;

        var navigationParameter = new ShellNavigationQueryParameters
        {
            { "Term", term }
        };
        _ = Shell.Current.GoToAsync(nameof(UpdateTermPage), navigationParameter);  
    }
}