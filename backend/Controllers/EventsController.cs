using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Timele.Dtos;
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
        [HttpPost("guess")]
        public ActionResult<GuessResponse> SubmitGuess([FromBody] GuessRequest request)
        {
            TimelineEvent? timelineEvent = Events.FirstOrDefault(e => e.Id == request.EventId);

            if (timelineEvent == null)
            {
                return NotFound("Event not found.");
            }

            if (request.GuessedYear < timelineEvent.Year)
            {
                return Ok(new GuessResponse
                {
                    Result = "TooEarly",
                    Message = "Too early! Try a later year.",
                    CorrectYear = timelineEvent.Year,
                    PointsEarned = 0
                });
            }

            if (request.GuessedYear > timelineEvent.Year)
            {
                return Ok(new GuessResponse
                {
                    Result = "TooLate",
                    Message = "Too late! Try an earlier year.",
                    CorrectYear = timelineEvent.Year,
                    PointsEarned = 0
                });
            }

            int points = timelineEvent.Difficulty switch
            {
                "Hard" => 30,
                "Medium" => 20,
                _ => 10
            };

            return Ok(new GuessResponse
            {
                Result = "Correct",
                Message = $"Correct! The answer was {timelineEvent.Year}.",
                CorrectYear = timelineEvent.Year,
                PointsEarned = points
            });
        }
    }
}