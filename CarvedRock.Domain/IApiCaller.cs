using CarvedRock.Core;

namespace CarvedRock.Domain;

public interface IApiCaller
{
    public Task<List<LocalClaim>?> CallExternalApiAsync();
}
