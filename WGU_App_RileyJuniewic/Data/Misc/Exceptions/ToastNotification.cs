using Ardalis.Result;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;

public class ToastNotification
{
	public ToastNotification(string message, bool isError = true)
	{
		ShowMessage(message, isError);
	}
	
	public ToastNotification(IEnumerable<string?> errors)
    {
        foreach (var error in errors)
		{
			if (error is null) continue;
            ShowMessage(error, true);
        }
    }

    private void ShowMessage(string message, bool isError)
	{
		MainThread.BeginInvokeOnMainThread(async () =>
		{
			var mainPage = Application.Current?.Windows[0].Page;
			if (mainPage is not null)
            {
				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

				ToastDuration duration = ToastDuration.Long;
				double fontSize = 16;

				var toastMessage = message;
				if (isError)
					toastMessage = $"Error: {message}";
					
				var toast = Toast.Make(toastMessage, duration, fontSize);


				await toast.Show(cancellationTokenSource.Token);
            }	
		});
	}
}