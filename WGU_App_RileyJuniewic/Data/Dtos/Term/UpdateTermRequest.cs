namespace WGU_App_RileyJuniewic.Data.Dtos.Term;

public class UpdateTermRequest : CreateTermRequest
{
    private Guid _termId;
    public Guid TermId
    {
        get => _termId;
        set
        {
            _termId = value;
            OnPropertyChanged(nameof(TermId));
            Validate(nameof(TermId), _termId);
        }
    }

    public UpdateTermRequest()
    {
        ValidateAll<UpdateTermRequest>();
    }
}