// Achievement Controller inherits from BaseController and has access to the IUnitOfWork and IMapper properties, which are injected into the BaseController constructor.

using AutoMapper;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.API.Controllers;

public class AchievementController : BaseController
{
    public AchievementController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    [HttpGet]
    [Route("{driverId:guid}")]
    public async Task<IActionResult> GetDriverAchievements(Guid driverId)
    {
        var driverAchievements = await _unitOfWork.AchievementsRepository.GetDriverAchievementAsync(driverId);
        if (driverAchievements == null)
        {
            return NotFound("Achievements not found");
        }

        var result = _mapper.Map<DriverAchievementResponse>(driverAchievements);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddAchievement([FromBody] CreateDriverAchievementRequest achievement)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = _mapper.Map<Achievement>(achievement);

        await _unitOfWork.AchievementsRepository.Add(result);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetDriverAchievements), new { driverId = result.DriverId }, result);

    }

    [HttpPut]
    public async Task<IActionResult> UpdateAchievements([FromBody] UpdateDriverAchievementRequest achievement)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = _mapper.Map<Achievement>(achievement);

        await _unitOfWork.AchievementsRepository.Update(result);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    [HttpDelete]
    [Route("{driverId:guid}")]
    public async Task<IActionResult> DeleteAchievement(Guid driverId)
    {
        await _unitOfWork.AchievementsRepository.Delete(driverId);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

}
