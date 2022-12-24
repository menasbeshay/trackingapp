using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackingApp.API.Areas.Auth.Models;
using TrackingApp.API.Models;
using TrackingApp.Core.Entities;
using TrackingApp.Infrastructure.Data;

namespace TrackingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : Controller
    {
        private readonly TAContext _context;

        public EventController(TAContext context)
        {
            _context = context;
        }
        [HttpPost("createEvent")]
        public async Task<IActionResult> createEvent([FromBody] EventModel oneEvent)
        {
            var EventExists = _context.events.Where(e => e.eventName == oneEvent.eventName).ToList();
            if (EventExists.Count > 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Message = "Event already exists!" });

            Event newEvent = new()
            {
                eventName = oneEvent.eventName,
                timeFrom = oneEvent.timeFrom,
                timeTo = oneEvent.timeTo,
                status = oneEvent.status,
                startFrom = oneEvent.startFrom,
                endsOn = oneEvent.endsOn,
                isRepeated = oneEvent.isRepeated,
                repeatedEvery = oneEvent.repeatedEvery,
            };
            _context.events.Add(newEvent);
            await _context.SaveChangesAsync();

            return Ok(new ResponseModel { Status = "Success", Message = "Event Id : " + newEvent.eventId + "Event created successfully!" });


        }

        [HttpPut("updateEvent")]
        public async Task<IActionResult> updateEvent([FromBody] Event oneEvent)
        {
            var EventExists = await _context.events.FirstOrDefaultAsync(e => e.eventId == oneEvent.eventId);
            if (EventExists == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Message = "Event Not exists!" });

            EventExists = new()
            {
                eventName = oneEvent.eventName,
                timeFrom = oneEvent.timeFrom,
                timeTo = oneEvent.timeTo,
                status = oneEvent.status,
                startFrom = oneEvent.startFrom,
                endsOn = oneEvent.endsOn,
                isRepeated = oneEvent.isRepeated,
                repeatedEvery = oneEvent.repeatedEvery,
            };
            var result = _context.events.Update(EventExists);
            await _context.SaveChangesAsync();
            //if (!result.State=="Success")
            //     return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Message = "Edit Event failed! Please check Event Data and try again." });

            return Ok(new ResponseModel { Status = "Success", Message = "Event Updated successfully!" });
        }

        [HttpGet("getEventbyDate")]
        public async Task<List<Event>> getEvent(int DateEvents)
        {
            var EventExists = _context.events.Where(e => e.startFrom == DateEvents).ToList();

            return EventExists;
        }

    }
}