using System.Collections.Generic;
using KittenFlipper.Entitites;

namespace KittenFlipper.Contracts
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
        bool IsValidUser(string userName, string password);
    }
}
