
using AutoMapper;
using Cepdi.Business.Medicines.Validations;
using Cepdi.Models.DTOs.Medicines;
using Cepdi.Models.Interfaces.Medicines;
using Cepdi.Models.Interfaces.Pharmacies;
using Cepdi.Models.Models;
using Cepdi.Models.Models.Medicines;
using Cepdi.Models.Models.Paginatores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cepdi.Business.Medicines
{
    public class MedicinesBussines : IMedicinesBusiness
    {
        private IMedicinesData _medicinesData;
        private readonly IMapper _mapper;
        private IPharmaciesBussines _pharmaciesBussines;

        public MedicinesBussines(IMedicinesData medicinesData, IMapper mapper, IPharmaciesBussines pharmaciesBussines)
        {
            this._medicinesData = medicinesData;
            this._mapper = mapper;
            this._pharmaciesBussines = pharmaciesBussines;
        }

        /// <summary>
        /// Crear un medicamento
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Retorna un true si creo el modelo</returns>
        public async Task<Response<bool>> Create(CreateMedicineModel model)
        {
            try
            {
                model.Habilitado = 1;
                var validatinPharmacy = _pharmaciesBussines.Get(model.IdFarmaceutica);
                if (validatinPharmacy == null)
                {
                    return new Response<bool> { Message = "La farmacia no es valida" };
                }

                var validation = HelperValidations.CreationValidations(model);

                

                if (!validation.Success)
                {
                    return new Response<bool> { Success = true, Message = validation.Message };
                }

                var create = await _medicinesData.Create(model);
                return new Response<bool> { Success = true, Data = create, Message = "Se creo correctamente"};
            }
            catch (System.Exception ex)
            {


                return new Response<bool>()
                {
                    Errors = new Errors
                    {
                        MethodName = $"Error en {nameof(Create)}",
                        PublicMessage = ex.Message,
                        TechnicalMessage = ex.StackTrace

                    }
                };
            }
        }

        /// <summary>
        /// Elimina un medicamento
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returna true si eliino el medicamento</returns>
        public async Task<Response<bool>> Delete(int id)
        {
            try
            {
                var result = await _medicinesData.Delete(id);
                return new Response<bool> { Success = true, Data = result, Message =  result ? string.Empty : "No se puedo realizar la acción" };
            }
            catch (System.Exception ex)
            {

                return new Response<bool>()
                {
                    Errors = new Errors
                    {
                        MethodName = $"Error en {nameof(Delete)}",
                        PublicMessage = ex.Message,
                        TechnicalMessage = ex.StackTrace

                    }
                };
            }
        }

        /// <summary>
        /// Obtiene un medicamento
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un la informacion del medicamento</returns>
        public async Task<Response<MedicineDTo>> Get(int id)
        {            
            try
            {
                var result = await _medicinesData.Get(id);
                return new Response<MedicineDTo>() 
                {
                    Success = true,
                    Data = _mapper.Map<MedicineDTo>(result)
                };
            }
            catch (System.Exception ex)
            {

                return new Response<MedicineDTo>()
                {
                    Errors = new Errors
                    {
                        MethodName = $"Error en {nameof(Get)}",
                        PublicMessage = ex.Message,
                        TechnicalMessage = ex.StackTrace

                    }
                };
            }

        }

        /// <summary>
        /// Obtiene todos los medicamentos
        /// </summary>
        /// <returns>Retorna la informacion de todos los medicamentos</returns>
        public async Task<Paginator<MedicineDTo>> GetAll(ShowTableMedicinesModel model)
        {
            var result = await _medicinesData.GetAll(model);

            var listdDto = _mapper.Map<List<MedicineDTo>>(result.registers);

            return new Paginator<MedicineDTo>()
            {
                Records = listdDto,
                TotalRecords = result.totalRecord,
                CurrentPage = model.Currentpage,
                RecordsPage = model.RecordPorPage
            };
        }

        /// <summary>
        /// Actualiza un medicamento
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Retorna el modelo con la nueva informacion que se actualizo</returns>
        public async Task<Response<bool>> Update(MedicineDTo model)
        {
            try
            {
                var MedicineDTo = _mapper.Map<MedicineModel>(model);
                var result = await _medicinesData.Update(MedicineDTo);

                return new Response<bool> { Success = true, Data = result, Message = result ? string.Empty : "Ocurrio un error al realizar la acción"};
            }
            catch (System.Exception ex)
            {

                return new Response<bool>()
                {
                    Errors = new Errors
                    {
                        MethodName = $"Error en {nameof(Update)}",
                        PublicMessage = ex.Message,
                        TechnicalMessage = ex.StackTrace

                    }
                };
            }
            
        }
    }
}
