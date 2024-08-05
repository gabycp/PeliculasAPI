using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Servicios;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext context,
                                    IMapper mapper,
                                    IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<List<PeliculaDTO>>> Get() 
        {
            var peliculas = await context.Peliculas.ToListAsync();

            return mapper.Map<List<PeliculaDTO>>(peliculas);
        }

        [HttpGet("{id}", Name = "obtenerPelicula")]
        public async Task<ActionResult<PeliculaDTO>> Get(int id) 
        {
            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if(pelicula == null) return NotFound();

            return mapper.Map<PeliculaDTO>(pelicula);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm]PeliculasCreacionDTO peliculasCreacionDTO) 
        {
            var pelicula = mapper.Map<Peliculas>(peliculasCreacionDTO);

            if (peliculasCreacionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculasCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculasCreacionDTO.Poster.FileName);
                    pelicula.Poster = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor,
                                           peliculasCreacionDTO.Poster.ContentType);
                }
            }

            context.Add(pelicula);
            await context.SaveChangesAsync();

            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);
            return new CreatedAtRouteResult("obtenerPelicula", new { id = pelicula.Id }, peliculaDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] PeliculasCreacionDTO peliculasCreacionDTO) 
        {
            var peliculaDB = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);
            if (peliculaDB == null)
            {
                return NotFound();
            }

            peliculaDB = mapper.Map(peliculasCreacionDTO, peliculaDB);

            if (peliculasCreacionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculasCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculasCreacionDTO.Poster.FileName);
                    peliculaDB.Poster = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor,
                                            peliculaDB.Poster,
                                           peliculasCreacionDTO.Poster.ContentType);
                }
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<PeliculaPatchDTO> patchDocument) 
        {
            if (patchDocument == null) return BadRequest();

            var entidadDB = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if (entidadDB == null) return NotFound();

            var entidadDTO = mapper.Map<PeliculaPatchDTO>(entidadDB);

            patchDocument.ApplyTo(entidadDTO, ModelState);

            var esValido = TryValidateModel(entidadDTO);

            if (!esValido) return BadRequest();

            mapper.Map(entidadDTO, entidadDB);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) 
        {
            var existePelicula = await context.Peliculas.AnyAsync(x => x.Id == id);
            if (!existePelicula)
            {
                return NotFound();
            }

            context.Remove(new Peliculas { Id = id });
            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}
