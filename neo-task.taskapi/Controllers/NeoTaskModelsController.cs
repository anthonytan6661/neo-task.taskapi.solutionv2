using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using neo_task.rabbitmq;
using neo_task.taskapi.Models;
using System.Text.Json;

namespace neo_task.taskapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NeoTaskModelsController : ControllerBase
    {
        private readonly NeoTaskModelContext _context;
        public readonly IPublisher _publisher;

        public NeoTaskModelsController(NeoTaskModelContext context, IPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        // GET: api/NeoTaskModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NeoTaskModel>>> GetNeoTasks()
        {
            return await _context.NeoTasks.ToListAsync();
        }

        // GET: api/NeoTaskModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NeoTaskModel>> GetNeoTaskModel(int id)
        {
            var neoTaskModel = await _context.NeoTasks.FindAsync(id);

            if (neoTaskModel == null)
            {
                return NotFound();
            }

            return neoTaskModel;
        }


        // POST: api/NeoTaskModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NeoTaskModel>> PostNeoTaskModel(NeoTaskModel neoTaskModel)
        {
            var message = neoTaskModel;
            _publisher.Publish(JsonSerializer.Serialize(message), "neo-task.tasks", null);
            _publisher.Publish(JsonSerializer.Serialize(message), "neo-task.tasks-backup", null);

            _context.NeoTasks.Add(neoTaskModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNeoTaskModel", new { id = neoTaskModel.Id }, neoTaskModel);
        }

     
       /* private bool NeoTaskModelExists(int id)
        {
            return _context.NeoTasks.Any(e => e.Id == id);
        }*/
    }
}
