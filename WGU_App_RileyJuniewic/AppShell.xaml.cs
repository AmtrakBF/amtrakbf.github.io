using WGU_App_RileyJuniewic.Forms.CourseForms;
using WGU_App_RileyJuniewic.Forms.TermForms;

namespace WGU_App_RileyJuniewic;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(AddCoursePage), typeof(AddCoursePage));
		Routing.RegisterRoute(nameof(AddTermPage), typeof(AddTermPage));
		Routing.RegisterRoute(nameof(UpdateTermPage), typeof(UpdateTermPage));
	}
}
