namespace WGU_App_RileyJuniewic.Data.Misc.Events;

public class GenericEventArgs<T> : EventArgs
{
    public T Value { get; set; }

    public GenericEventArgs(T value)
    {
        Value = value;
    }
}