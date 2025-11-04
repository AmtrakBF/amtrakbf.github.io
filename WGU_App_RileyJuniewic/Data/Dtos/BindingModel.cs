using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Ardalis.Result;

namespace WGU_App_RileyJuniewic.Data.Dtos;

public class BindingModel : INotifyDataErrorInfo, INotifyPropertyChanged
{
    protected Dictionary<string, List<string?>> _errors = [];

    public Dictionary<string, List<string?>> ValidationErrors
    {
        get => _errors;
        set
        {
            _errors = value;
            OnPropertyChanged(nameof(ValidationErrors));
        }
    }
    
    public bool HasErrors => ValidationErrors.Count > 0;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        if (propertyName is not null && ValidationErrors.ContainsKey(propertyName))
        {
            return ValidationErrors[propertyName];
        }

        return Enumerable.Empty<DataErrorsChangedEventArgs>();
    }

    public ErrorList GetErrorList()
    {
        var errors = new List<string?>();
        foreach (var error in ValidationErrors.Values)
        {
            errors.AddRange(error);
        }
        return new ErrorList(errors);
    }

    public IEnumerable<string?> GetAllErrors()
    {
        var errors = new List<string?>();
        foreach (var error in ValidationErrors.Values)
        {
            errors.AddRange(error);
        }
        return errors;
    }

    public virtual void Validate(string propertyName, object? propertyValue)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateProperty(propertyValue, new ValidationContext(this) { MemberName = propertyName }, results);

        if (results.Count > 0)
        {
            var errors = results.Select(r => r.ErrorMessage).ToList();
            ValidationErrors[propertyName] = errors;
        }
        else
        {
            ValidationErrors.Remove(propertyName);
        }
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        ValidationErrors = new Dictionary<string, List<string?>>(ValidationErrors);
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
            ValidationErrors.Remove(propertyName);

            var errors = results.Where(x => x.MemberNames.Contains(propertyName)).Select(r => r.ErrorMessage).ToList();
            if (errors is not null && errors.Count > 0)
                ValidationErrors.Add(propertyName, errors);

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        ValidationErrors = new Dictionary<string, List<string?>>(ValidationErrors);
    }
}