using AutoMapper;
using NetTopologySuite.Geometries;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory) 
        {
            CreateMap<Genero, GenerosDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();

            CreateMap<SalaDeCine, SalaDeCineDTO>()
                .ForMember(x => x.Latitud, x => x.MapFrom(y => y.Ubicacion.Y))
                .ForMember(x => x.Longitud, x => x.MapFrom(y => y.Ubicacion.X));

            CreateMap<SalaDeCineDTO, SalaDeCine>()
                .ForMember(x=> x.Ubicacion, x=> x.MapFrom( y => 
                 geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));



            CreateMap<SalaDeCineCreacionDTO, SalaDeCine>()
                .ForMember(x => x.Ubicacion, x => x.MapFrom(y =>
                 geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>()
                .ForMember( x=> x.Foto, options => options.Ignore());
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();

            CreateMap<Peliculas, PeliculaDTO>().ReverseMap();
            CreateMap<PeliculasCreacionDTO, Peliculas>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.peliculasGeneros, options => options.MapFrom(MapPeliculasGeneros))
                .ForMember(x => x.peliculasActores, options => options.MapFrom(MapPeliculasActores));
            CreateMap<Peliculas, PeliculasDetalleDTO>()
                .ForMember(x => x.Genero, options => options.MapFrom(MapPeliculasGeneros))
                .ForMember(x => x.Actores, options => options.MapFrom(MapPeliculasActores));
       
            CreateMap<PeliculaPatchDTO, Peliculas>().ReverseMap();

        }

        private List<ActorPeliculaDetalleDTO> MapPeliculasActores(Peliculas peliculas,PeliculasDetalleDTO peliculasDetalleDTO) 
        {
            var resultado = new List<ActorPeliculaDetalleDTO>();

            if(peliculas.peliculasActores == null) return resultado;

            foreach (var actor in peliculas.peliculasActores)
            {
                resultado.Add(new ActorPeliculaDetalleDTO()
                {
                 ActorId = actor.ActorId,
                 Personaje = actor.Personaje,
                 NombrePersona = actor.Actor.Nombre
                });
            }

            return resultado;
        }

        private List<GenerosDTO> MapPeliculasGeneros(Peliculas peliculas, PeliculasDetalleDTO peliculasDetalleDTO) 
        {
            var resultado = new List<GenerosDTO>();

            if( peliculas.peliculasGeneros == null ) return resultado;

            foreach (var genero in peliculas.peliculasGeneros)
            {
                resultado.Add(new GenerosDTO() { Id = genero.GeneroId, Nombre = genero.Genero.Nombre });
            }

            return resultado;
        }

        private List<PeliculasGeneros> MapPeliculasGeneros(PeliculasCreacionDTO peliculasCreacionDTO, Peliculas peliculas) 
        {
            var resultado = new List<PeliculasGeneros>();

            if(peliculasCreacionDTO.GeneroIds == null) return resultado;

            foreach (var id in peliculasCreacionDTO.GeneroIds) 
            {
                resultado.Add(new PeliculasGeneros() { GeneroId = id });
            }

            return resultado;   
        }

        private List<PeliculasActores> MapPeliculasActores(PeliculasCreacionDTO peliculasCreacionDTO, Peliculas peliculas)
        {
            var resultado = new List<PeliculasActores>();

            if(peliculasCreacionDTO.Actores == null) return resultado;

            foreach (var actor in peliculasCreacionDTO.Actores)
            {
                resultado.Add(new PeliculasActores() { ActorId = actor.ActorId, Personaje = actor.Personaje });
            }

            return resultado;
        }
    }
}
