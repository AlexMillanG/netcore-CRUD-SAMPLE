using SolucionChida.Domain.Entities;

namespace SolucionChida.Domain.Interfaces;

public interface IProductRepository
{
    public Task<Product?> GetProductByIdAsync(int id);

    public  Task<List<Product>> GetAllProductAsync();

    public Task<Product?> GeProductBySkuAsync(string sku);

    public Task SaveProductAsync(Product product);
    
    public Task DeleteProductAsync(int id);

    public  Task UpdateProductAsync(Product pr, int id);

    public Task<List<Product>> GetByCategoryId(int categoryId);
}