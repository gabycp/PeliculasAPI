using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Validaciones
{
    public class PesoArchivoValidacion: ValidationAttribute
    {
        private readonly int pesoMaxEnMegaByte;

        public PesoArchivoValidacion(int PesoMaxEnMegaByte) 
        {
            pesoMaxEnMegaByte = PesoMaxEnMegaByte;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null) 
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if(formFile.Length > pesoMaxEnMegaByte * 1024 * 1024) 
            {
                return new ValidationResult($"El peso del archivo no debe de ser mayor a { pesoMaxEnMegaByte } mb");
            }

            return ValidationResult.Success;
        }
    }
}
