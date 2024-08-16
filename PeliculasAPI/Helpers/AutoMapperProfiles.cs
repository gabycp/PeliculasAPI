using AutoMapper;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Genero, GenerosDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>()
                .ForMember( x=> x.Foto, options => options.Ignore());
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();

            CreateMap<Peliculas, PeliculaDTO>().ReverseMap();
            CreateMap<PeliculasCreacionDTO, Peliculas>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.peliculasGeneros, options => options.MapFrom(MapPeliculasGeneros));
            CreateMap<PeliculaPatchDTO, Peliculas>().ReverseMap();

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
    }
}
