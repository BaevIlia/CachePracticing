using CachePracticing.Domain.Dtos;
using CachePracticing.Domain.Repositories;
using CachePracticing.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace CachePracticing.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK ,Type = typeof(ProductViewModel))]
    public async Task<IActionResult> GetProduct([FromRoute] int id, CancellationToken ct = default)
    {
        var res = await _repository.Get(id, ct);

        return Ok(res);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<ProductViewModel>))]
    public async Task<IActionResult> GetProducts([FromQuery] int skip, int take, CancellationToken ct = default)
        => Ok(await _repository.GetList(skip, take, ct));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateProduct([FromBody] ProductData data, CancellationToken ct = default) 
    {
        await _repository.Create(data, ct);

        return NoContent();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] ProductUpdateData data, CancellationToken ct = default)
    {
        await _repository.Update(id, data.Name, data.Description, data.Price, ct);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteProduct([FromRoute] int id, CancellationToken ct = default)
    {
        await _repository.Delete(id, ct);

        return NoContent();
    }
}