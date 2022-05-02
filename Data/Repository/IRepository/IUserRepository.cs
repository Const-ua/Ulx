using Ulx.Models;

namespace Ulx.Data.Repository.IRepository
{

    public interface IUserRepository: IRepository<User>
    {
        User Find(string id);
    }
}
