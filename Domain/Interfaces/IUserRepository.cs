using SolucionChida.Domain.Entities;

namespace SolucionChida.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);

    Task<List<User>> FindAll();
    Task AddAsync(User user);

    Task UpdateAsync(User user, int id);

    Task DeleteAsync(int id);


}