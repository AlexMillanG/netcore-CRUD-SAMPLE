using Microsoft.EntityFrameworkCore;
using SolucionChida.Domain.Entities;
using SolucionChida.Domain.Interfaces;
using SolucionChida.Infrastructure.Data;

namespace SolucionChida.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    private UserRepository(AppDbContext context) => context = _context;

    public async Task<User?> GetByIdAsync(int id) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetByEmailAsync(string email)=>
        await _context.Users.FirstOrDefaultAsync(u => u.email == email);


    public async Task AddAsync(User user) =>
        await _context.Users.AddAsync(user);
}