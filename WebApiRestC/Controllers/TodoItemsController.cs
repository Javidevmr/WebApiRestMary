using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApiRestC.Models;
using WebApiRestC.RequestModels;

namespace WebApiRestC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly DatosTraceDBContext _context;
        private readonly ILogger<EventosController> logger;

        public TodoItemsController(DatosTraceDBContext context, ILogger<EventosController> logger)
        {
            //inyección del DBContext
            _context = context;
            //Inyección del logger
            this.logger = logger;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItem()
        {
            string evento = "GET";
            //Usamos el logger
            logger.LogInformation("Mary atención obteniendo los datos de TodoItems");
            TraceEF(evento, "obteniendo los datos de TodoItems");
            return await _context.TodoItem.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            string evento = $"GET/{id}";
            var todoItem = await _context.TodoItem.FindAsync(id);

            try
            {
                if (todoItem == null)
                {
                    logger.LogWarning($"No se puede encontrar el ID : {id}");
                    TraceEF(evento, $"No se puede encontrar el ID : {id}");
                    //se agrega un registro de  ClaseTrace (o seguimiento),
                    //con sus parámetros definidos. Se puede consultar en su controller.
                    return NotFound();
                }
                else
                {
                    //añadido Mary 15:04
                    logger.LogInformation($"Encontrado: {id}");
                    TraceEF(evento, $"Encontrado: {id}");
                    return todoItem;
                }
            }
            catch (Exception ex)
            {
                TraceEF(evento, ex.Message, 1);
                throw;
            }

        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, TodoItem_RequestInsert _todoItemrequest)
        {
            string evento = $"PUT/{id}";

            //busca el objeto según el ID desde la BD y le asigna los valores que se envían al endpoint a través de obj _todoItemrequest
            var todoItem = await _context.TodoItem.FindAsync(id);
            //condicion para asignar valores el objeto debe ser distino de NULL
            if (todoItem!=null)
            {
                todoItem.Name = _todoItemrequest.Name;
                todoItem.Detalle = _todoItemrequest.Detalle;
            }
            else
            {
                logger.LogWarning($"El ID : {id} no existe");
                TraceEF(evento, $"El ID : {id} no existe");
                return NotFound();
            }


            try
            {
                if (id != todoItem.Id)
                {
                    logger.LogWarning("BadRequest");
                    TraceEF(evento, "BadRequest");
                    return BadRequest();
                }

                _context.Entry(todoItem).State = EntityState.Modified;
                logger.LogWarning("EntityState ID Modificado");
                TraceEF(evento, $"EntityState ID Modificado : {id}");
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException dbex)
                {
                    if (!TodoItemExists(id))
                    {
                        logger.LogWarning($"No se puede encontrar el ID : {id}");
                        TraceEF(evento, dbex.Message, 1);
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                TraceEF(evento, ex.Message, 1);
                throw;
            }


        }

        // POST: api/TodoItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem_RequestInsert todoItem_insert)
        {
            string evento = "POST";

            //crear y llenar el objeto que recibe el EF
            TodoItem todoItem = new TodoItem();
            todoItem.Name = todoItem_insert.Name;
            todoItem.Detalle = todoItem_insert.Detalle;

            try
            {
                _context.TodoItem.Add(todoItem);
                
                //logger.LogDebug("Creado el id : " + todoItem.Id.ToString());
                await _context.SaveChangesAsync(); //crea el objeto en base de datos
                TraceEF(evento, "Creado el item  : " + todoItem.Id.ToString()); //guarda la acción en BD con el objeto creado
            }
            catch (Exception ex)
            {
                TraceEF(evento, ex.Message, 1);
                throw;
            }

            //devuelve el objeto creado
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(int id)
        {
            string evento = "DELETE";
            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItem.Remove(todoItem);
            await _context.SaveChangesAsync();
            TraceEF(evento, "Se eliminó el registro N°  : " + todoItem.Id.ToString()); //guarda la acción en BD con el objeto eliminado
            return todoItem;
        }

        private bool TodoItemExists(int id)
        {
            return _context.TodoItem.Any(e => e.Id == id);
        }

        private void TraceEF(string evento, string detalle = "", int eserror = 0)
        {
            _context.Eventos.Add(new Eventos { Evento = evento, Detalle = detalle, FechayHora = DateTime.Now, EsError = eserror });
            _context.SaveChanges();
        }
    }
}
