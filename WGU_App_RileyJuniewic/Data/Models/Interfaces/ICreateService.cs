using Ardalis.Result;

namespace WGU_App_RileyJuniewic.Data.Models.Interfaces;

public interface ICreateService<TRequest, TResponse>
{
    Task<Result<TResponse>> CreateAsync(TRequest request);
}