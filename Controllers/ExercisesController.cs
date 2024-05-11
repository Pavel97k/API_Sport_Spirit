using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Sport_Spirit.Model;
using Microsoft.AspNetCore.Authorization;
using System.Drawing.Printing;

namespace API_Sport_Spirit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExercisesController : ControllerBase
    {
        private readonly SportSpiritDatebaseContext _context;

        public ExercisesController(SportSpiritDatebaseContext context)
        {
            _context = context;
        }

        // GET: api/Exercises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetExercises(int page, int pageSize, int? CollectionId, bool? unique, bool? having, bool? logic)
        {
            var exercises = await _context.Exercises.Where(e => e.IsDeleted == false).ToListAsync();
            if (logic == true) exercises = await _context.Exercises.Where(e => e.IsDeleted == true).ToListAsync();
            if (logic == false) exercises = await _context.Exercises.Where(e => e.IsDeleted == false).ToListAsync();
            if (logic == null) exercises = await _context.Exercises.ToListAsync();

            if (CollectionId != 0) exercises = await _context.Exercises.Where(c => c.CollectionServerId == CollectionId).ToListAsync();
            if (unique.HasValue && unique.Value) exercises = await _context.Exercises.GroupBy(e => e.ExerciseName).Select(g => g.First()).ToListAsync();
            if (having.HasValue && having.Value) exercises = await _context.Exercises.GroupBy(e => e).Where(g => g.Count() > 1).SelectMany(g => g).ToListAsync();
            
            if (page == 0 || pageSize == 0) return exercises; 
            var paginationHelper = new PaginationHelper<Exercise>();
            return paginationHelper.Paginate(exercises, page, pageSize).ToList();

        }

        // GET: api/Exercises/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Exercise>> GetExercise(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);

            if (exercise == null || exercise.IsDeleted)
            {
                return NotFound();
            }

            return exercise;
        }

        // PUT: api/Exercises/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutExercise(int id, Exercise exercise)
        {
            if (id != exercise.IdExercise)
            {
                return BadRequest();
            }

            _context.Entry(exercise).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseExists(id))
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

        // POST: api/Exercises
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Exercise>> PostExercise(Exercise exercise)
        {
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExercise", new { id = exercise.IdExercise }, exercise);
        }

        [HttpPut("logic/{id}")]
        [Authorize]
        public async Task<ActionResult<Exercise>> PostLogicExercise(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise != null) exercise.IsDeleted = false;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Exercises/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteExercise(int id, bool logical)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }
            if (logical) exercise.IsDeleted = true;
            else _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExerciseExists(int id)
        {
            return _context.Exercises.Any(e => e.IdExercise == id);
        }
    }
}
