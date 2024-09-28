namespace PeliculasAPI.Entidades
{
    public class PeliculasSalaDeCines
    {
        public int PeliculaId { get; set; }
        public int SalaDeCineId { get; set; }
        public Peliculas Pelicula { get; set; }
        public SalaDeCine SalaDeCine { get; set; }
    }
}
