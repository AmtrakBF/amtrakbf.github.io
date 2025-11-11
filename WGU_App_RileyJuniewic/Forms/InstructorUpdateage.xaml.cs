using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Forms.InstructorForms;

public sealed partial class InstructorUpdatePage : ContentPage, IQueryAttributable
{
    private Instructor _instructor = new();
    public Instructor Instructor
    {
        get => _instructor;
        set
        {
            _instructor = value;
            OnPropertyChanged(nameof(Instructor));
        }
    }

    public InstructorUpdatePage()
    {
        InitializeComponent();
    }
    

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Instructor", out var instructorValue) && instructorValue is Instructor instructor)
        {
            Instructor = instructor;
        }
    }
}