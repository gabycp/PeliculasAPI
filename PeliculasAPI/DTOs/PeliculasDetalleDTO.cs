namespace PeliculasAPI.DTOs
{
    public class PeliculasDetalleDTO: PeliculaDTO
    {
        public List<GenerosDTO> Genero { get; set; }
        public List<ActorPeliculaDetalleDTO> Actores { get; set; }
    }
}
