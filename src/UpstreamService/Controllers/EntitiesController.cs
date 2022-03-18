using Microsoft.AspNetCore.Mvc;
using UpstreamService.Data;

namespace UpstreamService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntitiesController : ControllerBase
    {
        private readonly IRepository<SampleEntity> _repository;

        public EntitiesController(IRepository<SampleEntity> repository)
        {
            _repository = repository;
        }

        [HttpPost("{id:int}")]
        public async Task<ActionResult<string>> Create(int id, CreateEntityCommand command)
        {
            var entity = new SampleEntity(id, Who: command.Caller, Why: command.Why);
            await _repository.Put(entity.Id, entity);

            return Ok($"Entity with id {id} created successfully");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SampleEntity>> Get(int id)
        {
            try
            {
                return Ok(await _repository.Get(id));
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
