using PeliculasAPI.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class ActorCreacionDTO : ActorPatchDTO
    {
       
        [PesoArchivoValidacion(4)]
        [TipoArchivoValidacion(GrupoTipoArchivo.Imagen)]
        public IFormFile Foto { get; set; }
    }


}
