using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ActoresController( ApplicationDbContext context, IMapper mapper ) 
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get() 
        {
            var actores = await context.Actores.ToListAsync();
            return mapper.Map<List<ActorDTO>>(actores);

        }

        [HttpGet("{id:int}", Name = "obtenerActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id) 
        {
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if(actor == null) 
            {
                return NotFound();
            }

            var actorDTO = mapper.Map<ActorDTO>(actor);
            return actorDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO) 
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);
             context.Add(actor);
            //await context.SaveChangesAsync();

            var actorDTO = mapper.Map<ActorDTO>(actor);
            return new CreatedAtRouteResult("obtenerActor", new { id = actorDTO.Id }, actorDTO);

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);
            actor.Id = id;
            context.Entry(actor).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var existeActor = await context.Actores.AnyAsync(x=> x.Id == id);
            if (!existeActor) 
            {
                return NotFound();
            }

            context.Remove(new Actor { Id = id });
            await context.SaveChangesAsync();

            return NoContent();

        }




    }
}
