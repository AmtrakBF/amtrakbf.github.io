
namespace WGU_App_RileyJuniewic.Forms.Components.TermComponents;

public sealed partial class UpdateTermComponent : ModifyContentView
{
    public UpdateTermComponent()
    {
        this.InitializeComponent();
    }

    private void Close_View(object sender, EventArgs e) => OnCancel.Execute(sender);
}