using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Controllers
{
    [Route("api/SalasDeCine")]
    [ApiController]
    public class SalaDeCineController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public SalaDeCineController(ApplicationDbContext context,
               IMapper mapper):base(context, mapper) 
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<SalaDeCineDTO>>> Get() 
        {
            return await Get<SalaDeCine,SalaDeCineDTO>();
        }

        [HttpGet("{id:int}", Name = "obtenerSalaDeCine")]
        public async Task<ActionResult<SalaDeCineDTO>> Get(int id) 
        {
            return await Get<SalaDeCine, SalaDeCineDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult<SalaDeCineDTO>> Post([FromBody] SalaDeCineCreacionDTO salaDeCineCreacion) 
        {
            return await Post<SalaDeCineCreacionDTO, SalaDeCine, SalaDeCineDTO>
                (salaDeCineCreacion, "obtenerSalaDeCine");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] SalaDeCineCreacionDTO salaDeCineCreacionDTO)
        {
            return await Put<SalaDeCineCreacionDTO, SalaDeCine>(id,salaDeCineCreacionDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) 
        {
            return await Delete<SalaDeCine>(id);
        }

    }
}
