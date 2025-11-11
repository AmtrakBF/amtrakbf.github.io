using WGU_App_RileyJuniewic.Forms;
using WGU_App_RileyJuniewic.Forms.AssessmentForms;
using WGU_App_RileyJuniewic.Forms.CourseForms;
using WGU_App_RileyJuniewic.Forms.InstructorForms;
using WGU_App_RileyJuniewic.Forms.TermForms;

namespace WGU_App_RileyJuniewic;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(AddTermPage), typeof(AddTermPage));
		Routing.RegisterRoute(nameof(UpdateTermPage), typeof(UpdateTermPage));
		Routing.RegisterRoute(nameof(SelectedTermPage), typeof(SelectedTermPage));

		Routing.RegisterRoute(nameof(ModifyAssessmentPage), typeof(ModifyAssessmentPage));
		Routing.RegisterRoute(nameof(CreateAssessmentPage), typeof(CreateAssessmentPage));
		
		Routing.RegisterRoute(nameof(AddCoursePage), typeof(AddCoursePage));
		Routing.RegisterRoute(nameof(ViewCoursePage), typeof(ViewCoursePage));
		Routing.RegisterRoute(nameof(ModifyCoursePage), typeof(ModifyCoursePage));

		Routing.RegisterRoute(nameof(ModifyInstructorPage), typeof(ModifyInstructorPage));
		Routing.RegisterRoute(nameof(InstructorUpdatePage), typeof(InstructorUpdatePage));

		Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
	}
}
