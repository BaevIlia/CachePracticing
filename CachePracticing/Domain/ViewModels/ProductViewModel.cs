using AutoMapper;
using CachePracticing.Domain.Entities;

namespace CachePracticing.Domain.ViewModels;

[AutoMap(typeof(Product))]
public class ProductViewModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }
}