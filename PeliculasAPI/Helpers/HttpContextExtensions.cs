using Microsoft.EntityFrameworkCore;

namespace PeliculasAPI.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosPaginacion<T>(this HttpContext context, IQueryable<T> queryable,
            int cantidadRegistroPorPagina)
        {
            double cantidad = await queryable.CountAsync();
            double cantidadPagina = Math.Ceiling(cantidad / cantidadRegistroPorPagina);
           context.Response.Headers.Add("cantidadPagina", cantidadPagina.ToString());

        }
    }
}
