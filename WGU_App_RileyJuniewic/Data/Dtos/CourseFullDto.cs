namespace WGU_App_RileyJuniewic.Data.Dtos.Course;

public class FullCourseDto : BindingModel
{
    private Models.Course _course = new();
    public Models.Course Course
    {
        get => _course;
        set
        {
            _course = value;
            OnPropertyChanged(nameof(Course));
        }
    }
    
    private Models.Instructor _instructor = new();
    public Models.Instructor Instructor
    {
        get => _instructor;
        set
        {
            _instructor = value;
            OnPropertyChanged(nameof(Instructor));
        }
    }
}