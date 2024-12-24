// Driver Controller inherits from BaseController and has access to the IUnitOfWork and IMapper properties,
// which are injected into the BaseController constructor.

using AutoMapper;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using FormulaOne.Entities.Dtos.Requests;
using FormulaOne.Entities.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;


namespace FormulaOne.API.Controllers;

public class DriverController : BaseController
{
    public DriverController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    [HttpGet]
    [Route("{driverId:guid}")]
    public async Task<IActionResult> GetDriver(Guid driverId)
    {
        var driver = await _unitOfWork.DriverRepository.GetById(driverId);
        if (driver == null)
        {
            return NotFound("Driver not found");
        }

        var result = _mapper.Map<GetDriverResponse>(driver);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetDrivers()
    {
        var drivers = await _unitOfWork.DriverRepository.All();
        if (drivers == null)
        {
            return NotFound("Drivers not found");
        }

        var result = _mapper.Map<IEnumerable<GetDriverResponse>>(drivers);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddDriver([FromBody] CreateDriverRequest driver)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = _mapper.Map<Driver>(driver);

        await _unitOfWork.DriverRepository.Add(result);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetDriver), new { driverId = result.Id }, result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDriver([FromBody] UpdateDriverRequest driver)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = _mapper.Map<Driver>(driver);

        await _unitOfWork.DriverRepository.Update(result);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    [HttpDelete]
    [Route("{driverId:guid}")]
    public async Task<IActionResult> DeleteDriver(Guid driverId)
    {
        var driver = await _unitOfWork.DriverRepository.GetById(driverId);
        if (driver == null)
        {
            return NotFound("Driver not found");
        }

        await _unitOfWork.DriverRepository.Delete(driver.Id);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
}
