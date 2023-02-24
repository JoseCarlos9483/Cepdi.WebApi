using Cepdi.Models.Interfaces.Users;
using Cepdi.Models.Models;
using Cepdi.Models.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cepdi.Business.Users
{
    public  class UsersBussines : IUsersBusiness
    {
        /// <summary>
        /// Insumos
        /// </summary>
        private IUsersData _usersData;

        public UsersBussines(IUsersData usersData)
        {
            _usersData = usersData;
        }

        /// <summary>
        /// Verifica que el usuario tengo acceso
        /// </summary>
        /// <param name="loginRequest">Credenciales</param>
        /// <returns>Si el usuario tiene acceso</returns>
        public Response<LoginResponse> Login(LoginRequest loginRequest) {

            try
            {
                var validation = ValidationModel(loginRequest);

                if(validation.success){

                    var result = _usersData.Get(loginRequest);
                    
                    LoginResponse loginResponse = new LoginResponse()
                    {
                        Success = (result != null),
                        Token = ""
                    };

                    return new Response<LoginResponse> {
                        Success = true,
                        Data = loginResponse, 
                        Message = (result != null) ? string.Empty : "No se encontraton coincidencias"
                    };
                }

                return new Response<LoginResponse> { Message = validation.messagge };


            }
            catch (Exception ex)
            {

                return new Response<LoginResponse>
                {
                    Errors = new Errors
                    {
                        MethodName = $"Error en {nameof(Login)}",
                        PublicMessage = ex.Message,
                        TechnicalMessage = ex.StackTrace
                        
                    }
                };
            }
        }

        /// <summary>
        /// Validaciones del modelo
        /// </summary>
        /// <param name="loginRequest">Modelo a validar</param>
        /// <returns>Si el modelo es valido</returns>
        private (bool success, string messagge) ValidationModel(LoginRequest loginRequest) {

            return (string.IsNullOrEmpty(loginRequest.User) || string.IsNullOrEmpty(loginRequest.Password))? (false, "Ingrese todos los datos"):(true, "");
        }
        
        
    }
}
