using CachePracticing.Domain.Dtos;
using CachePracticing.Domain.ViewModels;

namespace CachePracticing.Domain.Repositories;

public interface IProductRepository
{
    Task<ProductViewModel> Get(int id, CancellationToken ct);

    Task<ICollection<ProductViewModel>> GetList(int skip, int take, CancellationToken ct);

    Task Create(ProductData data, CancellationToken ct);

    Task Update(int id, string name, string description, decimal price, CancellationToken ct);

    Task Delete(int id, CancellationToken ct);
}