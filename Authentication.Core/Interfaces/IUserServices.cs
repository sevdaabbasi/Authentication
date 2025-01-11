using Authentication.Core.Entities;

namespace Authentication.Core.Interfaces;

public interface IUserService
{
    Task AddUserAsync(User user);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
}