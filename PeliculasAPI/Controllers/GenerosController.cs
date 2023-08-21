using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenerosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenerosDTO>>> Get() 
        {
            var generos = await context.Generos.ToListAsync();
            var generosDto = mapper.Map<List<GenerosDTO>>(generos);
            return generosDto;
        }

        [HttpGet("{id:int}", Name = "obtenerGenero")]
        public async Task<ActionResult<GenerosDTO>> Get(int id) 
        {
            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);

            if(genero == null) 
            {
                return NotFound();
            }

            var generoDTO = mapper.Map<GenerosDTO>(genero);

            return generoDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO) 
        {
            var genero = mapper.Map<Genero>(generoCreacionDTO);

            context.Add(genero);
            await context.SaveChangesAsync();
            var generoDTO = mapper.Map<GenerosDTO>(genero);

            return new CreatedAtRouteResult("obtenerGenero", new { id = generoDTO.Id },generoDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoDTO) 
        {
            var genero = mapper.Map<Genero>(generoDTO);
            genero.Id = id;
            context.Entry(genero).State = EntityState.Modified;

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) 
        {
            var existe = await context.Generos.AnyAsync(x => x.Id == id);

            if (!existe) 
            {
                return NoContent();
            }

            context.Remove(new Genero { Id = id });
            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}
