namespace KittenFlipper.Contracts
{
    public interface IUserService
    {
        bool IsValidUser(string userName, string password);
    }
}
