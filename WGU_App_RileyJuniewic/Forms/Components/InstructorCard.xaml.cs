using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels.Course;

namespace WGU_App_RileyJuniewic.Forms.Components.InstructorComponents;

public sealed partial class InstructorCard : ContentView
{
    public static readonly BindableProperty SelectedInstructorProperty =
        BindableProperty.Create(nameof(SelectedInstructor), typeof(Instructor), typeof(InstructorCard), null, BindingMode.TwoWay, propertyChanged: OnSelectedInstructorChanged);

    private static void OnSelectedInstructorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is InstructorCard card && newValue is Instructor)
        {
            card.InstructorSelected = true;
            card.OnPropertyChanged(nameof(SelectedInstructor));
        } else
        {
            if (bindable is InstructorCard card2)
                card2.InstructorSelected = false;
        }
    }

    public Instructor? SelectedInstructor
    {
        get => (Instructor?)GetValue(SelectedInstructorProperty);
        set => SetValue(SelectedInstructorProperty, value);
    }

    private bool _instructorSelected;
    public bool InstructorSelected
    {
        get => _instructorSelected;
        set
        {
            _instructorSelected = value;
            OnPropertyChanged(nameof(InstructorSelected));
        }
    }

    private bool _modifyInstructor;
    public bool ModifyInstructor
    {
        get => _modifyInstructor;
        set
        {
            _modifyInstructor = value;
            OnPropertyChanged(nameof(ModifyInstructor));
        }
    }

    private bool _addNewInstructor;
    public bool AddNewInstructor
    {
        get => _addNewInstructor;
        set
        {
            _addNewInstructor = value;
            OnPropertyChanged(nameof(AddNewInstructor));
        }
    }
    
    private bool _chooseInstructor = true;
    public bool ChooseInstructor
    {
        get => _chooseInstructor;
        set
        {
            _chooseInstructor = value;
            OnPropertyChanged(nameof(ChooseInstructor));
        }
    }

    private InstructorCardViewModel _viewModel;

    public EventHandler? OnCancelEvent { get; set; }
    public EventHandler? OnModifyInstructorEvent { get; set; }

    public InstructorCard()
    {
        _viewModel = ServiceHelper.GetService<InstructorCardViewModel>();
        BindingContext = _viewModel;

        OnCancelEvent += (sender, e) => Show_Picked();
        OnModifyInstructorEvent += OnInstructorEventHandler;

        InitializeComponent();
    }

    private void OnInstructorEventHandler(object? sender, EventArgs e)
    {
        _ = _viewModel.GetAllInstructorsAsync();
        SelectedInstructor = null;
        InstructorSelected = false;
        Show_Picked();
    }

    private void Show_Picked()
    {
        AddNewInstructor = false;
        ModifyInstructor = false;
        ChooseInstructor = true;
    }

    private void Show_New_Instructor_Card(object sender, EventArgs e)
    {
        AddNewInstructor = true;
        ChooseInstructor = false;
    }

    private void Show_Update_Instructor_Card(object sender, EventArgs e)
    {
        ModifyInstructor = true;
        ChooseInstructor = false;
    }
}