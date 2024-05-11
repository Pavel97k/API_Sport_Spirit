using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Sport_Spirit.Model;
using Microsoft.AspNetCore.Authorization;

namespace API_Sport_Spirit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseCriterionsController : ControllerBase
    {
        private readonly SportSpiritDatebaseContext _context;

        public ExerciseCriterionsController(SportSpiritDatebaseContext context)
        {
            _context = context;
        }

        // GET: api/ExerciseCriterions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseCriterion>>> GetExerciseCriteria(int page, int pageSize, bool? logic)
        {
            var exercises_criteria = await _context.ExerciseCriteria.Where(e => e.IsDeleted == false).ToListAsync();
            if (logic == true) exercises_criteria = await _context.ExerciseCriteria.Where(e => e.IsDeleted == true).ToListAsync();
            if(logic == false) exercises_criteria = await _context.ExerciseCriteria.Where(e => e.IsDeleted == false).ToListAsync();
            if(logic == null) exercises_criteria = await _context.ExerciseCriteria.ToListAsync();

            if (page == 0 || pageSize == 0) return exercises_criteria;
            var paginationHelper = new PaginationHelper<ExerciseCriterion>();
            return paginationHelper.Paginate(exercises_criteria, page, pageSize).ToList();
        }

        // GET: api/ExerciseCriterions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseCriterion>> GetExerciseCriterion(int id)
        {
            var exerciseCriterion = await _context.ExerciseCriteria.FindAsync(id);

            if (exerciseCriterion == null || exerciseCriterion.IsDeleted == true)
            {
                return NotFound();
            }

            return exerciseCriterion;
        }

        // PUT: api/ExerciseCriterions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutExerciseCriterion(int id, ExerciseCriterion exerciseCriterion)
        {
            if (id != exerciseCriterion.IdExerciseCriteria)
            {
                return BadRequest();
            }

            _context.Entry(exerciseCriterion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseCriterionExists(id))
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

        // POST: api/ExerciseCriterions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ExerciseCriterion>> PostExerciseCriterion(ExerciseCriterion exerciseCriterion)
        {
            _context.ExerciseCriteria.Add(exerciseCriterion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExerciseCriterion", new { id = exerciseCriterion.IdExerciseCriteria }, exerciseCriterion);
        }

      
        [HttpPut("logic/{id}")]
        [Authorize]
        public async Task<ActionResult<ExerciseCriterion>> PostLogicExerciseCriterion(int id)
        {
            var exerciseCriterion = await _context.ExerciseCriteria.FindAsync(id);
            if (exerciseCriterion != null) exerciseCriterion.IsDeleted = false;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ExerciseCriterions/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteExerciseCriterion(int id, bool logical)
        {
            var exerciseCriterion = await _context.ExerciseCriteria.FindAsync(id);
            
            if (exerciseCriterion == null)
            {
                return NotFound();
            }
            if (logical) exerciseCriterion.IsDeleted = true;
            else _context.ExerciseCriteria.Remove(exerciseCriterion);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExerciseCriterionExists(int id)
        {
            return _context.ExerciseCriteria.Any(e => e.IdExerciseCriteria == id);
        }
    }
}
