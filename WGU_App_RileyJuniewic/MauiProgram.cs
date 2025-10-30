using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WGU_App_RileyJuniewic.Data.Repository;
using WGU_App_RileyJuniewic.Data.Services;
using WGU_App_RileyJuniewic.Data.ViewModels;
using WGU_App_RileyJuniewic.Forms;

namespace WGU_App_RileyJuniewic;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		using var stream = Assembly.GetExecutingAssembly()
			.GetManifestResourceStream("WGU_App_RileyJuniewic.appsettings.json");
		var config = new ConfigurationBuilder().AddJsonStream(stream!).Build();
		builder.Configuration.AddConfiguration(config);

		builder.Services.AddSingleton<HomePage>();
		builder.Services.AddSingleton(provider =>
		{
			var dbAccess = new SqlDataAccessAsync(config);
			_ = dbAccess.InitializeAsync();
			return dbAccess;
		});
		
		builder.Services.AddTransient<HomeViewModel>();

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
