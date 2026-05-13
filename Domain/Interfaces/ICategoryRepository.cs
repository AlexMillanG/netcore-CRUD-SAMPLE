using SolucionChida.Domain.DTOs;
using SolucionChida.Domain.Entities;

namespace SolucionChida.Domain.Interfaces;

public interface ICategoryRepository
{
    public Task CreateAsync (Category c);

    public Task UpdateAsync (Category c, int id);
    
    public Task<Category?> GetByIdAsync (int id);

    public Task<List<Category>> FindAllAsync();

    public Task DeleteAsync (int id);
    
    public Task<Category?> GetByNameAsync (string name);


}