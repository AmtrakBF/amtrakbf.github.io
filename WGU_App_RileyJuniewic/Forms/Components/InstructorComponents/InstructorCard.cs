using WGU_App_RileyJuniewic.Data.Misc.Commands;
using WGU_App_RileyJuniewic.Data.Misc.Events;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels.Course;

namespace WGU_App_RileyJuniewic.Forms.Components.InstructorComponents;

public sealed partial class InstructorCard : ContentView
{
    public static readonly BindableProperty SelectedInstructorProperty =
        BindableProperty.Create(nameof(SelectedInstructor), typeof(Instructor), typeof(InstructorCard), null, BindingMode.TwoWay);

    public Instructor? SelectedInstructor
    {
        get => (Instructor)GetValue(SelectedInstructorProperty);
        set
        {
            SetValue(SelectedInstructorProperty, value);
            InstructorSelected = true;
        }
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

    public OnClickCommand OnCancelCommand { get; set; }
    public EventHandler<InstructorEventArgs> OnModifyInstructorEvent { get; set; }
    public EventHandler OnDeleteInstructorEvent { get; set; }

    public InstructorCard()
    {
        _viewModel = ServiceHelper.GetService<InstructorCardViewModel>();
        BindingContext = _viewModel;

        OnCancelCommand = new OnClickCommand(Show_Picker);

        OnModifyInstructorEvent += OnInstructorEventHandler;
        OnDeleteInstructorEvent += OnInstructorEventHandler;

        InitializeComponent();
    }

    private void OnInstructorEventHandler(object? sender, EventArgs e)
    {
        _ = _viewModel.GetAllInstructorsAsync();
        SelectedInstructor = null;
        InstructorSelected = false;
        Show_Picker();
    }

    private void Show_Picker()
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