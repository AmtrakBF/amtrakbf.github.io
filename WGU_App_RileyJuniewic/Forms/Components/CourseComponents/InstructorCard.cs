using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels.Course;

namespace WGU_App_RileyJuniewic.Forms.Components.CourseComponents;

public sealed partial class InstructorCard : ContentView
{
    public static readonly BindableProperty SelectedInstructorProperty =
        BindableProperty.Create(nameof(SelectedInstructor), typeof(Instructor), typeof(InstructorCard), null);

    public Instructor SelectedInstructor
    {
        get => (Instructor)GetValue(SelectedInstructorProperty);
        set => SetValue(SelectedInstructorProperty, value);
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

    public InstructorCard()
    {
        InitializeComponent();

        var viewModel = ServiceHelper.GetService<InstructorCardViewModel>();
        BindingContext = viewModel;
        viewModel.OnCreateInstructor += (sender, args) =>
        {
            SelectedInstructor = args.Instructor;
            AddNewInstructor = false;
            ChooseInstructor = true;
        };

        this.InitializeComponent();
    }

    private void Show_New_Instructor_Card(object sender, EventArgs e)
    {
        AddNewInstructor = true;
        ChooseInstructor = false;
    }

    private void Show_Instructor_Picker(object sender, EventArgs e)
    {
        AddNewInstructor = false;
        ChooseInstructor = true;
    }
}