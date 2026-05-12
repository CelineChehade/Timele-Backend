using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Timele.Models;

namespace Timele.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private static readonly List<TimelineEvent> Events = new()
        {
            new TimelineEvent
            {
                Id = 1,
                Title = "Google was founded",
                Year = 1998,
                Category = "Technology",
                Difficulty = "Easy"
            },
            new TimelineEvent
            {
                Id = 2,
                Title = "The first iPhone was released",
                Year = 2007,
                Category = "Technology",
                Difficulty = "Easy"
            },
            new TimelineEvent
            {
                Id = 3,
                Title = "World War II ended",
                Year = 1945,
                Category = "History",
                Difficulty = "Easy"
            },
            new TimelineEvent
            {
                Id = 4,
                Title = "The Titanic sank",
                Year = 1912,
                Category = "History",
                Difficulty = "Easy"
            },
            new TimelineEvent
            {
                Id = 5,
                Title = "YouTube was founded",
                Year = 2005,
                Category = "Technology",
                Difficulty = "Easy"
            }
        };

        [HttpGet]
        public ActionResult<List<TimelineEvent>> GetAllEvents()
        {
            return Ok(Events);
        }

        [HttpGet("random")]
        public ActionResult<TimelineEvent> GetRandomEvent([FromQuery] string? category)
        {
            List<TimelineEvent> filteredEvents = Events;

            if (!string.IsNullOrWhiteSpace(category) && category != "All")
            {
                filteredEvents = Events
                    .Where(e => e.Category.ToLower() == category.ToLower())
                    .ToList();
            }

            if (filteredEvents.Count == 0)
            {
                return NotFound("No events found for this category.");
            }

            Random random = new Random();
            int randomIndex = random.Next(filteredEvents.Count);

            return Ok(filteredEvents[randomIndex]);
        }
    }
}