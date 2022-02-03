#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlatChecker.Data;
using FlatChecker.Models;
using Microsoft.AspNetCore.Authorization;
using FlatChecker.Models.Requests;
using FlatChecker.Models.Dto;
using FlatChecker.Models.Enums;
using FlatChecker.Utils;

namespace FlatChecker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestionsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public SuggestionsController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<SuggestionDto> GetSuggestion()
        {
            return _context.Suggestions.Select(ModelsMapUtils.Map);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuggestionDto>> GetSuggestion(int id)
        {
            var suggestion = await _context.Suggestions.FindAsync(id);

            if (suggestion == null)
            {
                return NotFound();
            }

            return ModelsMapUtils.Map(suggestion);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> PutSuggestion(EditSuggestionRequest request)
        {
            var suggestion = await _context.Suggestions
                .Where(s => s.Id == request.SuggestionId)
                .FirstOrDefaultAsync();

            if (suggestion.OwnerId.ToString() != User.Identity.Name)
            {
                return Forbid();
            }

            suggestion.Address = request.Address;
            suggestion.MonthPrice = request.MonthPrice;
            suggestion.District = request.District;
            suggestion.Floor = request.Floor; 
            suggestion.UpdatedDate = DateTime.UtcNow;

            try
            {
                _context.Suggestions.Update(suggestion);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuggestionExists(request.SuggestionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<SuggestionCreatedResponseDto>> PostSuggestion(
            AddSuggestionRequest request)
        {
            if (!int.TryParse(User.Identity.Name, out int curUserId))
            {
                return Unauthorized("Authorization failed");
            }

            Suggestion suggestion = Map(request);

            suggestion.OwnerId = curUserId;
            suggestion.UpdatedDate = DateTime.UtcNow;

            var added = _context.Suggestions.Add(suggestion).Entity;
            
            await _context.SaveChangesAsync();
            
            var minsFromPublicTransport = Map(request.MinsFromPublicTransport,
                                              added.Id);

            _context.MinsFromPublicTransportMaps.AddRange(minsFromPublicTransport);
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSuggestion", 
                new SuggestionCreatedResponseDto()
                {
                    SuggestionId = added.Id
                });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSuggestion(int id)
        {
            var suggestion = await _context.Suggestions.FindAsync(id);

            if (suggestion == null)
            {
                return NotFound();
            }
            if (!(suggestion.OwnerId.ToString() == User.Identity.Name
                || User.IsInRole(Role.Admin.ToString())))
            {
                return Forbid();
            }

            _context.Suggestions.Remove(suggestion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/settleInto")]
        [Authorize]
        public async Task<IActionResult> SettleIntoSuggestion(int id)
        {
            var suggestion = await _context.Suggestions.FindAsync(id);

            if (suggestion == null)
            {
                return NotFound();
            }
            if (suggestion.OwnerId.ToString() == User.Identity.Name
                || User.IsInRole(Role.Admin.ToString()))
            {
                return BadRequest("A property may not be settled by its owners or administrators");
            }
            if (suggestion.SettlerId != null)
            {
                return BadRequest("The property already has a settler");
            }
            if (!int.TryParse(User.Identity.Name, out int curUserId))
            {
                return Unauthorized("Authorization failed");
            }

            suggestion.SettlerId = curUserId;
            _context.Suggestions.Update(suggestion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpPost("{id}/leave")]
        [Authorize]
        public async Task<IActionResult> LeaveSuggestion(int id)
        {
            var suggestion = await _context.Suggestions.FindAsync(id);

            if (suggestion == null)
            {
                return NotFound();
            }
            if (suggestion.OwnerId.ToString() == User.Identity.Name
                || User.IsInRole(Role.Admin.ToString()))
            {
                return Forbid("A property may not be settled by its owners or administrators");
            }
            if (!int.TryParse(User.Identity.Name, out int curUserId))
            {
                return Unauthorized("Authorization failed");
            }

            suggestion.SettlerId = null;
            _context.Suggestions.Update(suggestion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static Suggestion Map(AddSuggestionRequest request)
        {
            return new Suggestion()
            {
                Address = request.Address,
                District = request.District,
                Floor = request.Floor,
                MonthPrice = request.MonthPrice
            };
        }

        private List<MinsFromPublicTransportMap> Map(
            List<MinsFromPublicTransportMapDto> minsFromPublicTransport,
            int suggestionId)
        {
            return minsFromPublicTransport.Select(mfpt => 
                new MinsFromPublicTransportMap()
                {
                    Mins = mfpt.Mins,
                    PublicTransportType = mfpt.PublicTransportType,
                    SuggestionId = suggestionId
                }).ToList();
        }

        private bool SuggestionExists(int id)
        {
            return _context.Suggestions.Any(e => e.Id == id);
        }
    }
}
