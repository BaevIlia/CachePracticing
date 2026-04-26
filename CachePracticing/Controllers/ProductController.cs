using CachePracticing.Domain.Dtos;
using CachePracticing.Domain.Repositories;
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
    public async Task<IActionResult> GetProduct([FromRoute] int id, CancellationToken ct = default)
    {
        var res = await _repository.Get(id, ct);

        return Ok(res);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] int take, int skip, CancellationToken ct = default)
        => Ok(await _repository.GetList(skip, take, ct));

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductData data, CancellationToken ct = default) 
    {
        await _repository.Create(data, ct);

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] string name, string description, decimal price, CancellationToken ct = default)
    {
        await _repository.Update(id, name, description, price, ct);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] int id, CancellationToken ct = default)
    {
        await _repository.Delete(id, ct);

        return NoContent();
    }
}