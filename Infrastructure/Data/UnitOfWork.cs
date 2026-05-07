using SolucionChida.Domain.Interfaces;

namespace SolucionChida.Infrastructure.Data;
 
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public UnitOfWork(AppDbContext context) => _context = context; 
    
    public async Task<int> SaveChangesAsync()=>
        await _context.SaveChangesAsync();
    
}