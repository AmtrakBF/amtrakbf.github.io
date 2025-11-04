using WGU_App_RileyJuniewic.Data.Misc.Events;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Forms.CourseForms;

namespace WGU_App_RileyJuniewic.Forms.Components.CourseComponents;

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

    public static readonly BindableProperty ButtonTitleProperty =
        BindableProperty.Create(nameof(ButtonTitle), typeof(string), typeof(CourseCard), null);

    public string ButtonTitle
    {
        get => (string)GetValue(ButtonTitleProperty);
        set => SetValue(ButtonTitleProperty, value);
    }

    public static readonly BindableProperty ButtonEventProperty =
        BindableProperty.Create(nameof(ButtonEvent), typeof(EventHandler), typeof(CourseCard), null, BindingMode.TwoWay);

    public EventHandler ButtonEvent
    {
        get => (EventHandler)GetValue(ButtonEventProperty);
        set => SetValue(ButtonEventProperty, value);
    }

    public CourseCard()
    {
        this.InitializeComponent();
    }

    private void On_Button_Click(object sender, EventArgs e)
    {
        var args = new CourseEventArgs(Course);
        ButtonEvent?.Invoke(this, args);
    }
}