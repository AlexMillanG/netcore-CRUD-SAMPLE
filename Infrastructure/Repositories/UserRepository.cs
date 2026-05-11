using Microsoft.EntityFrameworkCore;
using SolucionChida.Domain.Entities;
using SolucionChida.Domain.Interfaces;
using SolucionChida.Infrastructure.Data;

namespace SolucionChida.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) => _context = context;

    public async Task<User?> GetByIdAsync(int id) =>
        await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetByEmailAsync(string email)=>
        await _context.Users.Include(u=> u.Roles).FirstOrDefaultAsync(u => u.email == email);

    public async Task<List<User>> FindAll() =>
        await _context.Users.Include(u => u.Roles).ToListAsync();
    
    public async Task AddAsync(User user) =>
        await _context.Users.AddAsync(user);

    public async Task UpdateAsync(User user, int id)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u=> u.Id == id);

        if (existingUser == null)
        {
            return;
        }

        existingUser.name = user.name;
        existingUser.email = user.email;
        
    }

    public async Task DeleteAsync(int id) =>
        await _context.Users.Where(user => user.Id == id ).ExecuteDeleteAsync();
}