using WebAPI.Entities;
using System.ServiceModel;

namespace WebAPI.ServiceContract
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        string RegisterUser(User user);
    }
}
