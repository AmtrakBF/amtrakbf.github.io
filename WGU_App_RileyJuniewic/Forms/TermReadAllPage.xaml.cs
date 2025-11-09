using WGU_App_RileyJuniewic.Data.Misc.Events;
using WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;
using WGU_App_RileyJuniewic.Forms.TermForms;

namespace WGU_App_RileyJuniewic.Forms;

public partial class ViewTermsPage : ContentPage
{
    private readonly ViewAllTermsViewModel _viewModel;

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

    public EventHandler? OnViewEvent { get; set; }

    public ViewTermsPage(ViewAllTermsViewModel viewModel)
    {
        BindingContext = viewModel;
        _viewModel = viewModel;
        InitializeComponent();

        OnModifyEvent += Modify_Term_Clicked;
        OnViewEvent += View_Term_Clicked;
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
    
    private void View_Term_Clicked(object? sender, EventArgs e)
    {
        var termId = e as GuidEventArgs;
        if (termId is null) return;

        var navigationParameter = new ShellNavigationQueryParameters
        {
            { "TermId", termId.Value }
        };
        _ = Shell.Current.GoToAsync(nameof(SelectedTermPage), navigationParameter);
    }
}