using AutoMapper;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Servicios;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly string contenedor = "actores";
        //private readonly AlmacenadorArchivoAzure almacenadorArchivo;
        private readonly  IAlmacenadorArchivos almacenadorArchivos;

        public ActoresController( ApplicationDbContext context, IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos) 
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
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
            var entidad = mapper.Map<Actor>(actorCreacionDTO);

            if(actorCreacionDTO != null) 
            {
                using (var memoryStream = new MemoryStream()) 
                {
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    entidad.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor,
                                           actorCreacionDTO.Foto.ContentType);
                }
            }

            context.Add(entidad);
            await context.SaveChangesAsync();

            var actorDTO = mapper.Map<ActorDTO>(entidad);
            return new CreatedAtRouteResult("obtenerActor", new { id = actorDTO.Id }, actorDTO);

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actorDB = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if( actorDB == null) 
            {
                return NotFound();
            }

            actorDB = mapper.Map(actorCreacionDTO, actorDB);

            if (actorCreacionDTO.Foto != null) 
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    actorDB.Foto = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor,
                                            actorDB.Foto,
                                           actorCreacionDTO.Foto.ContentType);
                }
            }

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
