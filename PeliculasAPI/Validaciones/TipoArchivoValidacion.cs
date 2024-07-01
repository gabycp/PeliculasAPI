using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Validaciones
{
    public class TipoArchivoValidacion : ValidationAttribute
    {
        private readonly string[] tiposValido;

        public TipoArchivoValidacion(string[] tiposValido) 
        {
            this.tiposValido = tiposValido;
        }

        public TipoArchivoValidacion(GrupoTipoArchivo grupoTipoArchivo) 
        {
            if(grupoTipoArchivo == GrupoTipoArchivo.Imagen)
            {
                tiposValido = new string[] { "image/jpeg", "image/png", "image/gif" };
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if(!tiposValido.Contains(formFile.ContentType)) 
            {
                return new ValidationResult($"El tipo del archivo debe ser uno de los siguientes: {string.Join(",", tiposValido)}");
            }

            return ValidationResult.Success;
        }
    }
}
