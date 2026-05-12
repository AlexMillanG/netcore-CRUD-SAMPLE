using Microsoft.EntityFrameworkCore;
using SolucionChida.Domain.Entities;
using SolucionChida.Domain.Interfaces;
using SolucionChida.Infrastructure.Data;

namespace SolucionChida.Infrastructure.Repositories;

public class ProductRepository : IProductRepository 
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context) =>
        _context = context;

    public async Task<Product?> GetProductByIdAsync(int id) =>
         await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<List<Product>> GetAllProductAsync() =>
        await _context.Products.ToListAsync();

    public async Task<Product?> GeProductBySkuAsync(string sku) =>
        await _context.Products.FirstOrDefaultAsync(p => p.Sku == sku);

    public async Task SaveProductAsync(Product product) =>
        await _context.Products.AddAsync(product);

    public async Task DeleteProductAsync(int id) =>
        await _context.Products.Where(p => p.Id == id).ExecuteDeleteAsync();

    public async Task UpdateProductAsync(Product pr, int id)
    {
        var existing = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            return;
        }

        existing.Sku = pr.Sku;
        existing.Description = pr.Description;
        existing.Name = pr.Name;
    }
}