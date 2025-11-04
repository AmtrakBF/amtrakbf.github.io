using WGU_App_RileyJuniewic.Forms.AssessmentForms;
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

		Routing.RegisterRoute(nameof(ModifyAssessmentForm), typeof(ModifyAssessmentForm));
		Routing.RegisterRoute(nameof(CreateAssessmentForm), typeof(CreateAssessmentForm));
	}
}
