using WGU_App_RileyJuniewic.Data.ViewModels;

namespace WGU_App_RileyJuniewic.Forms;

public sealed partial class ReportsPage : ContentPage
{
    private readonly ReportsViewModel _reportsViewModel;

    public ReportsPage()
    {
        _reportsViewModel = ServiceHelper.GetService<ReportsViewModel>();
        BindingContext = _reportsViewModel;

        InitializeComponent();
    }

    override protected async void OnAppearing()
    {
        await _reportsViewModel.LoadReports();
    }
    
}