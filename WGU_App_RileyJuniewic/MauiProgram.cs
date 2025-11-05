using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WGU_App_RileyJuniewic.Data.Repository;
using WGU_App_RileyJuniewic.Data.Services;
using WGU_App_RileyJuniewic.Data.ViewModels;
using WGU_App_RileyJuniewic.Data.ViewModels.Course;
using WGU_App_RileyJuniewic.Forms;
using WGU_App_RileyJuniewic.Forms.CourseForms;
using CommunityToolkit.Maui;
using WGU_App_RileyJuniewic.Data.ViewModels.InstructorViewModels;
using WGU_App_RileyJuniewic.Data.ViewModels.CourseViewModes;
using WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;
using WGU_App_RileyJuniewic.Forms.TermForms;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Dtos.Assessment;
using WGU_App_RileyJuniewic.Data.Dtos.Term;
using WGU_App_RileyJuniewic.Data.Dtos.Instructor;
using WGU_App_RileyJuniewic.Data.Dtos.Course;
using WGU_App_RileyJuniewic.Data.ViewModels.AssessmentViewModels;


namespace WGU_App_RileyJuniewic;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		using var stream = Assembly.GetExecutingAssembly()
			.GetManifestResourceStream("WGU_App_RileyJuniewic.appsettings.json");
		var config = new ConfigurationBuilder().AddJsonStream(stream!).Build();
		builder.Configuration.AddConfiguration(config);

		builder.Services.AddSingleton(provider =>
		{
			var dbAccess = new SqlDataAccessAsync(config);
			_ = dbAccess.InitializeAsync();
			return dbAccess;
		});

		builder.Services.AddTransient<CurrentTermPage>();
		builder.Services.AddTransient<AddCoursePage>();
		builder.Services.AddTransient<AddTermPage>();
		builder.Services.AddTransient<UpdateTermPage>();
		
		builder.Services.AddTransient<CurrentTermViewModel>();
		builder.Services.AddTransient<ViewTermsViewModel>();
		builder.Services.AddTransient<AddTermViewModel>();
		builder.Services.AddTransient<UpdateTermViewModel>();
		
		builder.Services.AddTransient<InstructorCardViewModel>();
		builder.Services.AddTransient<UpdateInstructorViewModel>();
		builder.Services.AddTransient<AddInstructorViewModel>();

		builder.Services.AddTransient<AddCourseViewModel>();
		builder.Services.AddTransient<ViewCourseViewModel>();
		builder.Services.AddTransient<ModifyCourseViewModel>();

		builder.Services.AddTransient<CreateAssessmentViewModel>();
		builder.Services.AddTransient<ModifyAssessmentViewModel>();

		builder.Services.AddScoped<ICreateService<CreateAssessmentRequest, Assessment>, AssessmentService>();
		builder.Services.AddScoped<IModifyService<UpdateAssessmentRequest, Assessment>, AssessmentService>();


		builder.Services.AddScoped<ICreateService<CreateInstructorRequest, Instructor>, InstructorService>();
		builder.Services.AddScoped<IModifyService<UpdateInstructorRequest, Instructor>, InstructorService>();
		
		builder.Services.AddScoped<ICreateService<CreateCourseRequest, Course>, CourseService>();
		builder.Services.AddScoped<IModifyService<UpdateCourseRequest, Course>, CourseService>();

		builder.Services.AddScoped<ICreateService<CreateTermRequest, Term>, TermService>();
		builder.Services.AddScoped<IModifyService<UpdateTermRequest, Term>, TermService>();

		builder.Services.AddScoped<ITermService, TermService>();
		builder.Services.AddScoped<ICourseService, CourseService>();
		builder.Services.AddScoped<IInstructorService, InstructorService>();
		builder.Services.AddScoped<IAssessmentService, AssessmentService>();
		builder.Services.AddScoped<INoteService, NoteService>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

public static class ServiceHelper
{
	public static T GetService<T>() =>
		Current.GetService<T>() ?? throw new InvalidOperationException("Unable to locate the MAUI service provider.");

	public static IServiceProvider Current =>
		Application.Current?.Handler?.MauiContext?.Services
		?? throw new InvalidOperationException("Unable to locate the MAUI service provider.");
}