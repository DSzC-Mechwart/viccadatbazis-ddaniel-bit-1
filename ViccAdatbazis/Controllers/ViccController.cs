using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ViccAdatbazis.Data;
using ViccAdatbazis.Models;

namespace ViccAdatbazis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViccController : ControllerBase
    {
        private readonly ViccDbContext _context;

        public ViccController(ViccDbContext context)
        {
            _context = context;
        }

        //Viccek lekérdezése
        [HttpGet]
        //public async Task<ActionResult<List<Vicc>>> GetViccek()
        //{
        //    return await _context.Viccek.Where(x => x.Aktiv).ToListAsync();
        //}
        public async Task<ActionResult<string>> GetViccek()
        {
            return JsonSerializer.Serialize(_context.Viccek);
        }

        //Vicc lekérdezése

        [HttpGet("{id}")]
        public async Task<ActionResult<Vicc>> GetVicc(int id)
        {
            Vicc? vicc = await _context.Viccek.FindAsync(id);
            return vicc == null ? NotFound() : Ok(vicc);
        }


        //Új vicc feltöltése

        [HttpPost]
        public async Task<ActionResult<Vicc>> PostVicc(Vicc vicc)
        {
            await _context.Viccek.AddAsync(vicc);
            await _context.SaveChangesAsync();
            return Ok();
        }

        public async Task<ActionResult> PostVicc2(Vicc vicc)
        {
            await _context.Viccek.AddAsync(vicc);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetVicc", new { id = vicc.Id }, vicc);
        }

        //Meglévő vicc módosítása
        [HttpPut("{id}")]
        public async Task<ActionResult> PutVicc(int id, Vicc modositott)
        {
            if (id != modositott.Id) {
                return BadRequest();
            }
            _context.Entry(modositott).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();

        }


        //Vicc törlése

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVicc(int id)
        {
            Vicc? torlendo = await _context.Viccek.FindAsync(id);
            if (torlendo == null)
            {
                return NotFound();
            }

            if (torlendo.Aktiv)
            {
                torlendo.Aktiv = false;
                _context.Entry(torlendo).State = EntityState.Modified;
            }
            else
                _context.Viccek.Remove(torlendo);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //Lájkolás
        [Route("like/{id}")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> LajkVicc(int id)
        {
            Vicc? vicc = await _context.Viccek.FindAsync(id);
            if (vicc == null)
            {
                return NotFound();
            }
            vicc.Tetszik += 1;
            _context.Entry(vicc).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return CreatedAtAction("LajkVicc", new { id = vicc.Id }, vicc.Tetszik);
        }

        //Diszlájkolás
        [Route("dislike/{id}")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> DislajkVicc(int id)
        {
            Vicc? vicc = await _context.Viccek.FindAsync(id);
            if (vicc == null)
            {
                return NotFound();
            }
            vicc.NemTetszik += 1;
            _context.Entry(vicc).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return CreatedAtAction("DislajkVicc", new { id = vicc.Id }, vicc.NemTetszik);
        }
    }
}
