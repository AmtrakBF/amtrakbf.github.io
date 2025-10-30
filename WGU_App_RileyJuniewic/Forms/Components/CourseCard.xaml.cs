using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Forms.Components;

public sealed partial class CourseCard : ContentView
{
    public static readonly BindableProperty CourseProperty =
        BindableProperty.Create(nameof(Course), typeof(Course), typeof(CourseCard), null);

    public Course Course
    {
        get => (Course)GetValue(CourseProperty);
        set => SetValue(CourseProperty, value);
    }

    public static readonly BindableProperty InstructorProperty =
        BindableProperty.Create(nameof(Instructor), typeof(Instructor), typeof(CourseCard), null);

    public Instructor Instructor
    {
        get => (Instructor)GetValue(InstructorProperty);
        set => SetValue(InstructorProperty, value);
    }

    public CourseCard()
    {
        this.InitializeComponent();
    }
}