using WGU_App_RileyJuniewic.Data.ViewModels.InstructorViewModels;

namespace WGU_App_RileyJuniewic.Forms.Components.TermComponents;

public sealed partial class AddTermCard : ModifyContentView
{
    public AddTermCard()
    {
        var viewModel = ServiceHelper.GetService<AddInstructorViewModel>();
        BindingContext = viewModel;

        viewModel.OnCreateInstructor += (sender, args) =>
        {
            OnSubmit.Invoke(sender, args);
        };

        InitializeComponent();
    }

    private void Close_View(object sender, EventArgs e) => OnCancel.Execute(sender);
}