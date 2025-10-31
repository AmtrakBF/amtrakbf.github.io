using Ardalis.Result;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;

public class UserError
{
	public UserError(string message)
	{
		ShowErrorMessage(message);
	}
	
	public UserError(IEnumerable<string> errors)
    {
        foreach (var error in errors)
        {
            ShowErrorMessage(error);
        }
    }

    private void ShowErrorMessage(string message)
	{
		MainThread.BeginInvokeOnMainThread(async () =>
		{
			var mainPage = Application.Current?.Windows[0].Page;
			if (mainPage is not null)
            {
				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

				ToastDuration duration = ToastDuration.Long;
				double fontSize = 16;

				var toast = Toast.Make(message, duration, fontSize);

				await toast.Show(cancellationTokenSource.Token);
            }	
		});
	}
}