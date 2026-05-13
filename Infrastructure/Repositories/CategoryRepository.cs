using Microsoft.EntityFrameworkCore;
using SolucionChida.Domain.DTOs;
using SolucionChida.Domain.Entities;
using SolucionChida.Domain.Interfaces;
using SolucionChida.Infrastructure.Data;

namespace SolucionChida.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Category c) =>
        await _context.Categories.AddAsync(c);
    

    public async Task UpdateAsync(Category c, int id)
    {
        var existing = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (existing == null)
        {
            return;
        }
        existing.Name = c.Name;
    }

    public async Task<Category?> GetByIdAsync(int id) =>
        await _context.Categories.FirstOrDefaultAsync( c => c.Id == id);

    public async Task<List<Category>> FindAllAsync() =>
        await _context.Categories.ToListAsync();

    public async Task DeleteAsync(int id) =>
        await _context.Categories.Where(x => x.Id == id).ExecuteDeleteAsync();

    public async Task<Category?> GetByNameAsync(string name) =>
        await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
}