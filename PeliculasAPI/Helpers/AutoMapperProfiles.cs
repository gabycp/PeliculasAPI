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
                .ForMember(x => x.Poster, options => options.Ignore());
            CreateMap<PeliculaPatchDTO, Peliculas>().ReverseMap();

        }
    }
}
