namespace PeliculasAPI.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int cantidadRegistroPorPagina = 10;
        private readonly int cantidadMaximaPorPagina = 50;

        public int CantidadRegistroPorPagina 
        {
            get => cantidadRegistroPorPagina;
            set 
            {
                cantidadRegistroPorPagina = (value > cantidadMaximaPorPagina) ? cantidadMaximaPorPagina : value; 
            }
        }
    }
}
