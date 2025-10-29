using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WGU_App_RileyJuniewic.Data.Dtos;

public class BindingDto : INotifyDataErrorInfo, INotifyPropertyChanged
{
    protected Dictionary<string, List<string?>> _errors = [];
    public bool HasErrors => _errors.Count > 0;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        if (propertyName is not null && _errors.ContainsKey(propertyName))
        {
            return _errors[propertyName];
        }

        return Enumerable.Empty<DataErrorsChangedEventArgs>();
    }

    public virtual void Validate(string propertyName, object? propertyValue)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateProperty(propertyValue, new ValidationContext(this) { MemberName = propertyName }, results);

        if (results.Count > 0)
        {
            _errors.Remove(propertyName);

            var errors = results.Select(r => r.ErrorMessage).ToList();
            if (errors is not null && errors.Count > 0)
                _errors.Add(propertyName, errors);

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        else
        {
            _errors.Remove(propertyName);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }

    public virtual void ValidateAll<T>()
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(this, new ValidationContext(this), results, true);

        var props = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.DeclaringType == typeof(T))
            .Select(p => p.Name);

        foreach (var propertyName in props)
        {
            _errors.Remove(propertyName);

            var errors = results.Where(x => x.MemberNames.Contains(propertyName)).Select(r => r.ErrorMessage).ToList();
            if (errors is not null && errors.Count > 0)
                _errors.Add(propertyName, errors);

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}