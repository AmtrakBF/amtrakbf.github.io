using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels.InstructorViewModels;

namespace WGU_App_RileyJuniewic.Forms.Components.InstructorComponents;

public sealed partial class UpdateInstructorCard : ModifyContentView
{

    public static readonly BindableProperty InstructorProperty =
        BindableProperty.Create(nameof(Instructor), typeof(Instructor), typeof(UpdateInstructorCard), null, BindingMode.TwoWay, propertyChanged: OnInstructorChanged);

    public Instructor Instructor
    {
        get => (Instructor)GetValue(InstructorProperty);
        set
        {
            SetValue(InstructorProperty, value);
            _viewModel.SetInstructor(value);
        }
    }

    public static readonly BindableProperty OnDeleteProperty =
        BindableProperty.Create(nameof(OnDelete), typeof(EventHandler), typeof(ModifyContentView), null, BindingMode.TwoWay);

    public EventHandler OnDelete
    {
        get => (EventHandler)GetValue(OnDeleteProperty);
        set => SetValue(OnDeleteProperty, value);
    }

    private UpdateInstructorViewModel _viewModel;

    public UpdateInstructorCard()
    {
        _viewModel = ServiceHelper.GetService<UpdateInstructorViewModel>();
        BindingContext = _viewModel;

        _viewModel.OnUpdateInstructor += (sender, args) => OnSubmit?.Invoke(sender, args);
        _viewModel.OnDeleteInstructor += (sender, args) => OnDelete?.Invoke(sender, args);

        InitializeComponent();
    }

    private void Close_View(object sender, EventArgs e) => OnCancel?.Invoke(sender, e);

    private static void OnInstructorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var card = (UpdateInstructorCard)bindable;
        if (newValue is Instructor instructor)
            card._viewModel.SetInstructor(instructor);
    }
}