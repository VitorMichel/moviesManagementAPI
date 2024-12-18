using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieManagementApi.Data;
using MovieManagementApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly MovieDbContext _context;

    public MoviesController(MovieDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
    {
        var moviesWithRatings = await _context.Movies.ToListAsync();

        return Ok(moviesWithRatings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovieById(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
            return NotFound();

        return movie;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
    {
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(int id, Movie movie)
    {
        if (id != movie.Id)
            return BadRequest();

        _context.Entry(movie).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
            return NotFound();

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchMovies([FromQuery] string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest("Name parameter is required for searching.");
        }

        var movies = await _context.Movies
            .Where(m => EF.Functions.Like(m.Title, $"%{name}%"))
            .ToListAsync();

        return Ok(movies);
    }

    [HttpGet("{movieId}/Actors")]
    public async Task<IActionResult> GetActorsByMovie(int movieId)
    {
        var movie = await _context.Movies
            .Include(a => a.MoviesActors)
            .ThenInclude(a => a.Actor)
            .FirstOrDefaultAsync(m => m.Id == movieId);

        if (movie == null)
            return NotFound("Movie not found");

        var actor = movie.MoviesActors.Select(ma => new
        {
            ma.Actor.Id,
            ma.Actor.Name
        });

        return Ok(actor);
    }
}
