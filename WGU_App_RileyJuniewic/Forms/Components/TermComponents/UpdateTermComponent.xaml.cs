
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;

namespace WGU_App_RileyJuniewic.Forms.Components.TermComponents;

public sealed partial class UpdateTermComponent : ModifyContentView
{
    private readonly UpdateTermViewModel _viewModel;

    public static readonly BindableProperty TermProperty =
        BindableProperty.Create(nameof(Term), typeof(Term), typeof(UpdateTermComponent), null, BindingMode.TwoWay, propertyChanged: OnTermChanged);

    public Term Term
    {
        get => (Term)GetValue(TermProperty);
        set
        {
            SetValue(TermProperty, value);
            _viewModel.SetTerm(value);
        }
    }

    public UpdateTermComponent()
    {
        var viewModel = ServiceHelper.GetService<UpdateTermViewModel>();
        BindingContext = viewModel;
        _viewModel = viewModel;

        viewModel.OnUpdateTerm += (sender, args) =>
        {
            OnSubmit?.Invoke(sender, args);
        };

        viewModel.OnDeleteTerm += (sender, args) =>
        {
            OnSubmit?.Invoke(sender, args);
        };


        InitializeComponent();
    }

    private void Close_View(object sender, EventArgs e) => OnSubmit?.Invoke(sender, e);

     private static void OnTermChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var card = (UpdateTermComponent)bindable;
        if (newValue is Term term)
            card._viewModel.SetTerm(term);
    }
}