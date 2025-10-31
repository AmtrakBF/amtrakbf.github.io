using System.Windows.Input;

namespace WGU_App_RileyJuniewic.Data.Misc.Commands;

public class OnClickCommand(Action action, Predicate<object>? canExecute = null) : ICommand
{

    public event EventHandler? CanExecuteChanged;

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, new EventArgs());
    }

    public bool CanExecute(object? parameter)
    {
        if (canExecute is not null)
            return canExecute(parameter!);
        return true;
    }

    public void Execute(object? parameter)
    {
        action();
    }
}

public class OnClickCommandAsync(Func<Task> action, Predicate<object>? canExecute = null) : ICommand
{

    public event EventHandler? CanExecuteChanged;

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, new EventArgs());
    }

    public bool CanExecute(object? parameter)
    {
        if (canExecute is not null)
            return canExecute(parameter!);
        return true;
    }

    public void Execute(object? parameter)
    {
        _ = action();
    }
}