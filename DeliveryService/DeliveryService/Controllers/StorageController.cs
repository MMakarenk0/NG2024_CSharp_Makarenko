using AutoMapper;
using BLL.Models.AddEntityModels;
using BLL.Models.UpdateEntityModels;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Controllers;

[ApiController]
[Route("[controller]")]
public class StorageController : Controller
{
    private readonly IStorageService _storageService;
    private readonly IMapper _mapper;

    public StorageController(IStorageService storageService, IMapper mapper)
    {
        _storageService = storageService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var storages = await _storageService.GetAllAsync();
        return Ok(storages);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var storage = await _storageService.GetByIdAsync(id);
        return Ok(storage);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(SaveStorageModel model)
    {
        var storage = await _storageService.AddAsync(model);
        return Ok(storage);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateStorageModel model)
    {
        var storage = await _storageService.UpdateAsync(model);
        return Ok(storage);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _storageService.DeleteAsync(id);
        return NoContent();
    }
}