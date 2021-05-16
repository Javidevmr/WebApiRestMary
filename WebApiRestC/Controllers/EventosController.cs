using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRestC.Models;

namespace WebApiRestC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly DatosTraceDBContext _context;

        public EventosController(DatosTraceDBContext context)
        {
            _context = context;
        }

        // GET: api/Eventos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Eventos>>> GetEventos()
        {
            return await _context.Eventos.ToListAsync();
        }

        // GET: api/Eventos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Eventos>> GetEventos(int id)
        {
            var eventos = await _context.Eventos.FindAsync(id);

            if (eventos == null)
            {
                return NotFound();
            }

            return eventos;
        }

        //// PUT: api/Eventos/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutEventos(int id, Eventos eventos)
        //{
        //    if (id != eventos.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(eventos).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EventosExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Eventos
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<Eventos>> PostEventos(Eventos eventos)
        //{
        //    _context.Eventos.Add(eventos);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetEventos", new { id = eventos.Id }, eventos);
        //}

        //// DELETE: api/Eventos/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Eventos>> DeleteEventos(int id)
        //{
        //    var eventos = await _context.Eventos.FindAsync(id);
        //    if (eventos == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Eventos.Remove(eventos);
        //    await _context.SaveChangesAsync();

        //    return eventos;
        //}

        private bool EventosExists(int id)
        {
            return _context.Eventos.Any(e => e.Id == id);
        }
    }
}
