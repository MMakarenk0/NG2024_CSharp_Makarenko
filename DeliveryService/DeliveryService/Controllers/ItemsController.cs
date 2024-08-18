using AutoMapper;
using BLL.Models.AddEntityModels;
using BLL.Models.UpdateEntityModels;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemController : Controller
{
    private readonly IItemService _itemService;
    private readonly IMapper _mapper;

    public ItemController(IItemService itemService, IMapper mapper)
    {
        _itemService = itemService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var items = await _itemService.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await _itemService.GetByIdAsync(id);
        return Ok(item);
    }

    [HttpGet("sender/{senderPhone}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySenderPhone(string senderPhone)
    {
        var items = await _itemService.GetBySenderPhone(senderPhone);
        return Ok(items);
    }

    [HttpGet("receiver/{receiverPhone}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByReceiverPhone(string receiverPhone)
    {
        var items = await _itemService.GetByRecieverPhone(receiverPhone);
        return Ok(items);
    }

    [HttpGet("filters")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByFilters(string? description, float? minWeight, float? maxWeight, [FromQuery] List<Guid> categoryIds, DateTime? startDate, DateTime? endDate)
    {
        var items = await _itemService.GetByFilters(description, minWeight, maxWeight, categoryIds, startDate, endDate);
        return Ok(items);
    }

    [HttpGet("storage/{storageId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByStorage(Guid storageId)
    {
        var items = await _itemService.GetByStorage(storageId);
        return Ok(items);
    }

    [HttpPut("status/{itemId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItemStatus(Guid itemId, bool? isRecieved)
    {
        var item = await _itemService.UpdateItemStatus(itemId, isRecieved);
        return Ok(item);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(SaveItemModel model)
    {
        var item = await _itemService.AddAsync(model);
        return Ok(item);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateItemModel model)
    {
        var item = await _itemService.UpdateAsync(model);
        return Ok(item);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _itemService.DeleteAsync(id);
        return NoContent();
    }
}
