using Microsoft.EntityFrameworkCore;
using SolucionChida.Domain.Entities;
using SolucionChida.Domain.Interfaces;
using SolucionChida.Infrastructure.Data;

namespace SolucionChida.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context) =>
        _context = context;


    public async Task<Role?> GetByIdAsync(int id) =>
        await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);

    public async Task<List<Role>> GetByIdsAsync(List<int> ids) =>
        await _context.Roles.Where(r => ids.Contains(r.Id)).ToListAsync();
}