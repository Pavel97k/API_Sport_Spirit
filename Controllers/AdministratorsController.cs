using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Sport_Spirit.Model;
using Microsoft.AspNetCore.Authorization;

namespace API_Sport_Spirit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorsController : ControllerBase   
    {
        private readonly JwtService _jwtService;

        private readonly SportSpiritDatebaseContext _context;

        public AdministratorsController(SportSpiritDatebaseContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // GET: api/Administrators
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Administrator>>> GetAdministrators()
        {
            return await _context.Administrators.ToListAsync();
        }

        // GET: api/Administrators/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Administrator>> GetAdministrator(int id)
        {
            var administrator = await _context.Administrators.FindAsync(id);

            if (administrator == null)
            {
                return NotFound();
            }

            return administrator;
        }

        // PUT: api/Administrators/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAdministrator(int id, Administrator administrator)
        {
            if (id != administrator.IdAdministrator)
            {
                return BadRequest();
            }

            _context.Entry(administrator).State = EntityState.Modified;

            try
            {
                var user = _context.Administrators.FirstOrDefaultAsync(x => x.IdAdministrator == administrator.IdAdministrator);
                if (administrator.Password != null)
                {
                    administrator.Password = Password_Security.ComputeHash(administrator.Password);
                }else { administrator.Password = user.Result.Password; }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdministratorExists(id))
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

        // POST: api/Administrators/authorization
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("authorization")]
        public async Task<IActionResult> AdministratorAuthorization([FromBody] AuthenticateUser authenticateUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Administrators.FirstOrDefaultAsync(x => x.Login == authenticateUser.Login);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            authenticateUser.Password = Password_Security.ComputeHash(authenticateUser.Password);
            if (user.Password != authenticateUser.Password)
            {
                return BadRequest("Passwords do not match");
            }

            var token = _jwtService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        // POST: api/Administrators/register
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("register")]
        [Authorize]
        public async Task<IActionResult> AdministratorRegister([FromBody] Administrator administrator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Administrators.FirstOrDefaultAsync(x => x.Login == administrator.Login);
            if (user != null)
            {
                return BadRequest("User found");
            }

            administrator.Password = Password_Security.ComputeHash(administrator.Password);

            _context.Administrators.Add(administrator);

            await _context.SaveChangesAsync();

            return Ok(new { administrator });
        }

        // DELETE: api/Administrators/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAdministrator(int id)
        {
            var administrator = await _context.Administrators.FindAsync(id);
            if (administrator == null)
            {
                return NotFound();
            }

            _context.Administrators.Remove(administrator);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdministratorExists(int id)
        {
            return _context.Administrators.Any(e => e.IdAdministrator == id);
        }
    }
}
