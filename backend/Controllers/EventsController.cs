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
            new TimelineEvent { Id = 1, Title = "Google was founded", Year = 1998, Category = "Technology", Difficulty = "Easy" },
            new TimelineEvent { Id = 2, Title = "The first iPhone was released", Year = 2007, Category = "Technology", Difficulty = "Easy" },
            new TimelineEvent { Id = 3, Title = "YouTube was founded", Year = 2005, Category = "Technology", Difficulty = "Easy" },
            new TimelineEvent { Id = 4, Title = "Netflix was founded", Year = 1997, Category = "Technology", Difficulty = "Medium" },

            new TimelineEvent { Id = 5, Title = "World War II ended", Year = 1945, Category = "History", Difficulty = "Easy" },
            new TimelineEvent { Id = 6, Title = "The Titanic sank", Year = 1912, Category = "History", Difficulty = "Easy" },
            new TimelineEvent { Id = 7, Title = "The Berlin Wall fell", Year = 1989, Category = "History", Difficulty = "Medium" },
            new TimelineEvent { Id = 8, Title = "Lebanon gained independence", Year = 1943, Category = "History", Difficulty = "Medium" },

            new TimelineEvent { Id = 9, Title = "The moon landing occurred", Year = 1969, Category = "Science", Difficulty = "Easy" },
            new TimelineEvent { Id = 10, Title = "Albert Einstein published the theory of relativity", Year = 1905, Category = "Science", Difficulty = "Hard" },
            new TimelineEvent { Id = 11, Title = "The first successful airplane flight happened", Year = 1903, Category = "Science", Difficulty = "Medium" },

            new TimelineEvent { Id = 12, Title = "The first Harry Potter book was published", Year = 1997, Category = "Books", Difficulty = "Medium" },
            new TimelineEvent { Id = 13, Title = "The Great Gatsby was published", Year = 1925, Category = "Books", Difficulty = "Hard" },

            new TimelineEvent { Id = 14, Title = "The PlayStation 2 was released", Year = 2000, Category = "Gaming", Difficulty = "Medium" },
            new TimelineEvent { Id = 15, Title = "Minecraft was officially released", Year = 2011, Category = "Gaming", Difficulty = "Easy" },

            new TimelineEvent { Id = 16, Title = "The movie Titanic was released", Year = 1997, Category = "Movies", Difficulty = "Easy" },
            new TimelineEvent { Id = 17, Title = "Avatar was released", Year = 2009, Category = "Movies", Difficulty = "Easy" },

            new TimelineEvent { Id = 18, Title = "The first FIFA World Cup was held", Year = 1930, Category = "Sports", Difficulty = "Hard" },
            new TimelineEvent { Id = 19, Title = "The first modern Olympic Games were held", Year = 1896, Category = "Sports", Difficulty = "Hard" },

            new TimelineEvent { Id = 20, Title = "Michael Jackson released Thriller", Year = 1982, Category = "Music", Difficulty = "Medium" }
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
        [HttpPost]
        public ActionResult<TimelineEvent> CreateEvent([FromBody] TimelineEvent newEvent)
        {
            if (newEvent == null)
            {
                return BadRequest("Event data is required.");
            }

            if (string.IsNullOrWhiteSpace(newEvent.Title))
            {
                return BadRequest("Event title is required.");
            }

            if (newEvent.Year <= 0)
            {
                return BadRequest("A valid year is required.");
            }

            if (string.IsNullOrWhiteSpace(newEvent.Category))
            {
                return BadRequest("Category is required.");
            }

            if (string.IsNullOrWhiteSpace(newEvent.Difficulty))
            {
                return BadRequest("Difficulty is required.");
            }

            bool alreadyExists = Events.Any(e =>
                e.Title.Trim().ToLower() == newEvent.Title.Trim().ToLower());

            if (alreadyExists)
            {
                return BadRequest("Event already exists.");
            }

            int nextId = Events.Count == 0 ? 1 : Events.Max(e => e.Id) + 1;

            newEvent.Id = nextId;

            Events.Add(newEvent);

            return CreatedAtAction(nameof(GetAllEvents), new { id = newEvent.Id }, newEvent);
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteEvent(int id)
        {
            TimelineEvent? timelineEvent = Events.FirstOrDefault(e => e.Id == id);

            if (timelineEvent == null)
            {
                return NotFound("Event not found.");
            }

            Events.Remove(timelineEvent);

            return NoContent();
        }
    }
}