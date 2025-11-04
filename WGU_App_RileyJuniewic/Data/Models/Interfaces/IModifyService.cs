using Ardalis.Result;

namespace WGU_App_RileyJuniewic.Data.Models.Interfaces;

public interface IModifyService<TRequest, TResponse>
{
    Task<Result<TResponse>> UpdateAsync(TRequest request);
    Task<Result> DeleteAsync(Guid id);
}