using SolucionChida.Domain.Entities;

namespace SolucionChida.Domain.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int Id);
    Task<List<Role>> GetByIdsAsync(List<int> ids);
}