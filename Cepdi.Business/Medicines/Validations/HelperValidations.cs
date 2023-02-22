using Cepdi.Models.Models;

namespace Cepdi.Business.Medicines.Validations
{
    public static class HelperValidations
    {
        /// <summary>
        /// Valida que los modelos 
        /// </summary>
        /// <param name="model">Cualquier modelo</param>
        /// <returns>Una clase con la respuesta de validacion</returns>
        public static ValidationModel CreationValidations(object model)
        {
            string message = string.Empty;
            var prueba = model.GetType().GetProperties();
            foreach (var item in prueba) {

                var valor = item.GetValue(model);
                string nombre = item.Name;
                string tipo = item.PropertyType.Name;

                if (valor == null) {
                    message += $"La propiedad {nombre} no es valido";
                }
            }

            return new ValidationModel() {
                Success = string.IsNullOrEmpty(message),
                Message = message
            };
        }


    }
}
