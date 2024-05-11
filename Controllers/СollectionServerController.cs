using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Sport_Spirit.Model;
using Microsoft.AspNetCore.Authorization;

namespace API_Sport_Spirit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class СollectionServerController : ControllerBase
    {
        private readonly SportSpiritDatebaseContext _context;

        public СollectionServerController(SportSpiritDatebaseContext context)
        {
            _context = context;
        }

        // GET: api/СollectionServer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollectionServer>>> GetСollectionServers(int page, int pageSize, int gender, bool? logic)
        {
            var collections = await _context.CollectionServers.Where(e => e.IsDeleted == false).ToListAsync();

            if (logic == true) collections = await _context.CollectionServers.Where(e => e.IsDeleted == true).ToListAsync();
            if (logic == false) collections = await _context.CollectionServers.Where(e => e.IsDeleted == false).ToListAsync();
            if (logic == null) collections = await _context.CollectionServers.ToListAsync();

            if (gender != 0) collections = collections.Where(c => c.GenderId == gender).ToList();
            if (page == 0 || pageSize == 0) return collections;
            var pagination = new PaginationHelper<CollectionServer>();
            return pagination.Paginate(collections, page, pageSize).ToList();
        }



        // GET: api/СollectionServer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CollectionServer>> GetСollectionServer(int id)
        {
            var сollectionServer = await _context.CollectionServers.FindAsync(id);

            if (сollectionServer == null || сollectionServer.IsDeleted == true)
            {
                return NotFound();
            }

            return сollectionServer;
        }

        // PUT: api/СollectionServer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutСollectionServer(int id, CollectionServer сollectionServer)
        {
            if (id != сollectionServer.IdCollectionServer)
            {
                return BadRequest();
            }

            _context.Entry(сollectionServer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!СollectionServerExists(id))
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

        // POST: api/СollectionServer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CollectionServer>> PostСollectionServer(CollectionServer сollectionServer)
        {
            _context.CollectionServers.Add(сollectionServer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetСollectionServer", new { id = сollectionServer.IdCollectionServer }, сollectionServer);
        }

        [HttpPut("logic/{id}")]
        [Authorize]
        public async Task<ActionResult<CollectionServer>> PostLogicCollectionServer(int id)
        {
            var collectionServer = await _context.CollectionServers.FindAsync(id);
            if (collectionServer != null) collectionServer.IsDeleted = false;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/СollectionServer/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteСollectionServer(int id, bool logical)
        {
            var сollectionServer = await _context.CollectionServers.FindAsync(id);
            if (сollectionServer == null)
            {
                return NotFound();
            }
            if (logical) сollectionServer.IsDeleted = true;
            else _context.CollectionServers.Remove(сollectionServer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool СollectionServerExists(int id)
        {
            return _context.CollectionServers.Any(e => e.IdCollectionServer == id);
        }
    }
}
