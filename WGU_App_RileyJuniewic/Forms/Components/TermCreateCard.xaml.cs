using WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;

namespace WGU_App_RileyJuniewic.Forms.Components.TermComponents;

public sealed partial class AddTermCard : ModifyContentView
{
    public AddTermCard()
    {
        var viewModel = ServiceHelper.GetService<AddTermViewModel>();
        BindingContext = viewModel;

        viewModel.OnCreate += (sender, args) =>
        {
            OnSubmit?.Invoke(sender, args);
        };

        InitializeComponent();
    }

    private void Close_View(object sender, EventArgs e) => OnSubmit?.Invoke(sender, e);
}