using WGU_App_RileyJuniewic.Forms.CourseForms;

namespace WGU_App_RileyJuniewic;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("AddCoursePage", typeof(AddCoursePage));
	}
}
