using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1.Models;

namespace Q1.Controllers
{
    [Route("api/director")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        private readonly PePrnFall22B1Context _context;

        public DirectorsController(PePrnFall22B1Context pePrnFall22B1)
        {
            _context = pePrnFall22B1;
        }

        [HttpGet("getdirectors/{nationality}/{gender}")]
        public IActionResult GetDirectors(string nationality, string gender)
        {
            var directors = _context.Directors.ToList();

            var result = directors
                .Where(d => d.Nationality.Equals(nationality, StringComparison.CurrentCultureIgnoreCase))
                .Where(d => d.Male == (gender.Equals("Male", StringComparison.CurrentCultureIgnoreCase)))
                .Select(d => new
                {
                    d.Id,
                    d.FullName,
                    gender = d.Male ? "Male" : "Female",
                    dob = d.Dob.ToDateTime(TimeOnly.MinValue),
                    dobString = d.Dob.ToString("M/d/yyy"),
                    d.Nationality,
                    d.Description
                });

            return Ok(result);
        }

        [HttpGet("getdirector/{id}")]
        public IActionResult GetDirector(int id)
        {
            var director = _context.Directors
                .Include(d => d.Movies)
                    .ThenInclude(m => m.Producer)

                .Select(d => new
                {
                    d.Id,
                    d.FullName,
                    gender = d.Male ? "Male" : "Female",
                    dob = d.Dob.ToDateTime(TimeOnly.MinValue),
                    dobString = d.Dob.ToString("M/d/yyy"),
                    d.Nationality,
                    d.Description,
                    movies = d.Movies.Select(m => new
                    {
                        m.Id,
                        m.Title,
                        releaseDate = m.ReleaseDate.Value.ToDateTime(TimeOnly.MinValue),
                        releaseYear = m.ReleaseDate.Value.Year,
                        m.Description,
                        m.Language,
                        m.ProducerId,
                        m.DirectorId,
                        producerName = m.Producer.Name,
                        directorName = d.FullName,
                        genres = Array.Empty<object>(),
                        stars = Array.Empty<object>(),
                    })
                })
                .FirstOrDefault(d => d.Id == id);

            return Ok(director);
        }

        [HttpPost("create")]
        public IActionResult Create(CreateDto dto)
        {
            try
            {
                _context.Directors.Add(new Director
                {
                    FullName = dto.FullName,
                    Description = dto.Description,
                    Male = dto.Male,
                    Dob = dto.Dob,
                    Nationality = dto.Nationality
                });

                return Ok(_context.SaveChanges());

            }
            catch (Exception ex)
            {
                return Conflict("There is an error while adding.");
            }
        }

        public class CreateDto
        {
            public string FullName { get; set; }

            public bool Male { get; set; }

            public DateOnly Dob { get; set; }

            public string Nationality { get; set; }

            public string Description { get; set; }
        }



    }
}
