using RestApiCore.Models;

namespace RestApiCore.Services.Interfaces
{
    public interface IAuthenticateService
    {
        LoggedUser Authenticate (string username, string password);
    }
}
