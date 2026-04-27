using AutoMapper;
using CachePracticing.Domain.Dtos;
using CachePracticing.Domain.Entities;
using CachePracticing.Domain.ViewModels;
using CachePracticing.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CachePracticing.Domain.Repositories;

public class CachedProductRepository : IProductRepository
{
    private readonly ProductsDbContext _dbContext;
    private readonly IMapper _mappper;
    private readonly IDistributedCache _cache;

    public CachedProductRepository(ProductsDbContext dbContext, IMapper mappper, IDistributedCache cache)
    {
        _dbContext = dbContext;
        _mappper = mappper;
        _cache = cache;
    }

    public async Task Create(ProductData data, CancellationToken ct = default)
    {
        var product = new Product
        {
            Name = data.Name,
            Description = data.Description,
            Price = data.Price,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        await _dbContext.Products.AddAsync(product, ct);

        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task Delete(int id, CancellationToken ct = default)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id, ct);

        _dbContext.Products.Remove(product);

        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<ProductViewModel> Get(int id, CancellationToken ct = default)
    {
        var productFromCache = await _cache.GetStringAsync($"product:{id}");

        if (!string.IsNullOrEmpty(productFromCache))
            return _mappper.Map<ProductViewModel>(JsonConvert.DeserializeObject<Product>(productFromCache));

        var productFromDb = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id, ct);

        await _cache.SetStringAsync($"product:{id}", JsonConvert.SerializeObject(productFromDb));

        return _mappper.Map<ProductViewModel>(productFromDb);
    }

    public async Task<ICollection<ProductViewModel>> GetList(int skip, int take, CancellationToken ct)
    {
        var res = await _dbContext.Products.Skip(skip).Take(take).ToListAsync(ct);

        return _mappper.Map<ICollection<ProductViewModel>>(res);
    }

    public async Task Update(int id, string name, string description, decimal price, CancellationToken ct = default)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id, ct);

        product.Name = name;
        product.Description = description;
        product.Price = price;
        product.UpdatedAt = DateTime.Now;

        await _dbContext.SaveChangesAsync(ct);
    }
}