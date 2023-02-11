using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            try
            {

                var EventExists = await _context.events.Where(e => e.eventName == oneEvent.eventName).ToListAsync();
                if (EventExists.Count > 0)
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Message = "Event already exists!" });
                string RepeatedEvery = oneEvent.repeatedEvery;
                int ResultId = 0;
                if (RepeatedEvery == "Once")
                {
                    ResultId = int.Parse(CreateOnceEvent(oneEvent).Result.ToString());

                }
                else if (RepeatedEvery == "Daily")
                {
                    ResultId = int.Parse(CreateDailyEvent(oneEvent).Result.ToString());
                }
                else if (RepeatedEvery == "Weekly")
                {
                    ResultId = int.Parse(CreateWeeklyEvent(oneEvent).Result.ToString());
                }
                else if (RepeatedEvery == "Monthly")
                {
                    ResultId = int.Parse(CreateMonthlyEvent(oneEvent).Result.ToString());
                }

                return Ok(new ResponseModel { Status = "Success", Message = "Event Id : " + ResultId + "Event created successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("updateEvent")]
        public async Task<IActionResult> updateEvent([FromBody] Event oneEvent)
        {
            try
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
            catch   (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("RemoveEvent")]
        public async Task<IActionResult> RemoveEvent([FromBody] Event oneEvent)
        {
            var EventExists = await _context.events.FirstOrDefaultAsync(e => e.eventId == oneEvent.eventId);
            if (EventExists == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Message = "Event Not exists!" });
           
            var result = _context.events.Remove(EventExists);
            await _context.SaveChangesAsync();
            //if (!result.State=="Success")
            //     return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Message = "Edit Event failed! Please check Event Data and try again." });
            return Ok(new ResponseModel { Status = "Success", Message = "Event Removed successfully!" });
        }
        [HttpGet("GetAllEvents")]
        public async Task<List<Event>> GetAllEvents()//linefrom 
        {
            var EventExists = await _context.events.ToListAsync();
            return EventExists;
        }
        [HttpGet("getEventbyDate")]
        public async Task<List<Event>> getEventsInRange(int FromDateEvents, int ToDateEvents)//linefrom 
        {
            var EventExists =await  _context.events.Where(e => e.startFrom >= FromDateEvents && e.endsOn <= ToDateEvents).ToListAsync();
            return EventExists;
        }
        async Task<int> CreateOnceEvent(EventModel oneEvent)
        {
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
            return newEvent.eventId;
        }
        async Task<int> CreateDailyEvent(EventModel oneEvent)
        {
            DateTime EndDate = ConvertUnixTimeStampToDateTime(oneEvent.startFrom);
            // DateTime StartDate = ConvertUnixTimeStampToDateTime(oneEvent.startFrom);
            long EndDateTimeStamp = oneEvent.startFrom;
            while ( EndDateTimeStamp<=oneEvent.endsOn)
            {
                Event newEvent = new()
                {
                    eventName = oneEvent.eventName,
                    timeFrom = oneEvent.timeFrom,
                    timeTo = oneEvent.timeTo,
                    status = oneEvent.status,
                    startFrom = oneEvent.startFrom,
                    endsOn = EndDateTimeStamp,
                    isRepeated = oneEvent.isRepeated,
                    repeatedEvery = oneEvent.repeatedEvery,
                };
                _context.events.Add(newEvent);
                EndDate = EndDate.AddDays(1);
                EndDateTimeStamp = ConvertToTimestamp(EndDate);
            }

            await _context.SaveChangesAsync();
            return 1;
        }
        async Task<int> CreateWeeklyEvent(EventModel oneEvent)
        {
            DateTime EndDate = ConvertUnixTimeStampToDateTime(oneEvent.startFrom);
            // DateTime StartDate = ConvertUnixTimeStampToDateTime(oneEvent.startFrom);
            long EndDateTimeStamp = oneEvent.startFrom;
            while (EndDateTimeStamp <= oneEvent.endsOn)
            {
                Event newEvent = new()
                {
                    eventName = oneEvent.eventName,
                    timeFrom = oneEvent.timeFrom,
                    timeTo = oneEvent.timeTo,
                    status = oneEvent.status,
                    startFrom = oneEvent.startFrom,
                    endsOn = EndDateTimeStamp,
                    isRepeated = oneEvent.isRepeated,
                    repeatedEvery = oneEvent.repeatedEvery,
                };
                _context.events.Add(newEvent);
                EndDate = EndDate.AddDays(7);
                EndDateTimeStamp = ConvertToTimestamp(EndDate);
            }

            await _context.SaveChangesAsync();
            return 1;
        }
        async Task<int> CreateMonthlyEvent(EventModel oneEvent)
        {
            DateTime EndDate = ConvertUnixTimeStampToDateTime(oneEvent.startFrom);
            // DateTime StartDate = ConvertUnixTimeStampToDateTime(oneEvent.startFrom);
            long EndDateTimeStamp = oneEvent.startFrom;
            while (EndDateTimeStamp <= oneEvent.endsOn)
            {
                Event newEvent = new()
                {
                    eventName = oneEvent.eventName,
                    timeFrom = oneEvent.timeFrom,
                    timeTo = oneEvent.timeTo,
                    status = oneEvent.status,
                    startFrom = oneEvent.startFrom,
                    endsOn = EndDateTimeStamp,
                    isRepeated = oneEvent.isRepeated,
                    repeatedEvery = oneEvent.repeatedEvery,
                };
                _context.events.Add(newEvent);
                EndDate = EndDate.AddMonths(1);
                EndDateTimeStamp = ConvertToTimestamp(EndDate);
            }

            await _context.SaveChangesAsync();
            return 1;
        }

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static long ConvertToTimestamp(DateTime value)
        {
            TimeSpan elapsedTime = value - Epoch;
            return (long)elapsedTime.TotalSeconds;
        }
        public static DateTime ConvertUnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}