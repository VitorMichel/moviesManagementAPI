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
public class ActorsController : ControllerBase
{
    private readonly MovieDbContext _context;

    public ActorsController(MovieDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetActors()
    {
        var actors = await _context.Actors.ToListAsync();
        return Ok(actors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Actor>> GetActorById(int id)
    {
        var actor = await _context.Actors.FindAsync(id);

        if (actor == null)
            return NotFound();

        return actor;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Actor>> CreateActor(Actor actor)
    {
        _context.Actors.Add(actor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetActorById), new { id = actor.Id }, actor);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActor(int id, Actor actor)
    {
        if (id != actor.Id)
            return BadRequest();

        _context.Entry(actor).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActor(int id)
    {
        var actor = await _context.Actors.FindAsync(id);
        if (actor == null)
            return NotFound();

        _context.Actors.Remove(actor);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchActors([FromQuery] string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest("Name parameter is required for searching.");
        }

        var actors = await _context.Actors
            .Where(a => EF.Functions.Like(a.Name, $"%{name}%"))
            .ToListAsync();

        return Ok(actors);
    }

    [HttpGet("{actorId}/Movies")]
    public async Task<IActionResult> GetMoviesByActor(int actorId)
    {
        var actor = await _context.Actors
            .Include(a => a.MoviesActors)
            .ThenInclude(ma => ma.Movie)
            .FirstOrDefaultAsync(a => a.Id == actorId);

        if (actor == null)
            return NotFound("Actor not found");

        var movies = actor.MoviesActors.Select(ma => new
        {
            ma.Movie.Id,
            ma.Movie.Title
        });

        return Ok(movies);
    }
}