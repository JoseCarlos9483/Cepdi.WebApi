using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Cepdi.Models.Interfaces.Medicines;
using System.Threading.Tasks;
using Cepdi.Models.Models.Medicines;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Cepdi.Models.DTOs.Medicines;
using Cepdi.Models.Models.Paginatores;
using Cepdi.Models.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cepdi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        private IMedicinesBusiness _medicinesBusiness;
    

        public MedicinesController(IMedicinesBusiness medicinesBusiness)
        {
            this._medicinesBusiness = medicinesBusiness;
        }

        [HttpGet("{current}/{record}")]       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Paginator<MedicineDTo>> GetAll(int current, int record ) 
            => await _medicinesBusiness.GetAll(new ShowTableMedicinesModel() { Currentpage = current, RecordPorPage = record });

        
        // GET api/<MedicinesController>/5
        [HttpGet("{id}")]
        public async Task<Response<MedicineDTo>> Get(int id)
        => await _medicinesBusiness.Get(id);



        // POST api/<MedicinesController>
        [HttpPost]
        [Route("Create")]
        public async Task<Response<bool>> Post([FromBody] CreateMedicineModel model)
        => await _medicinesBusiness.Create(model);

        // PUT api/<MedicinesController>/5
        [HttpPut("{id}")]
        public async Task<Response<bool>> Put(int id, [FromBody] MedicineDTo model)
        {
            model.IdMedicamento = id;
            return await _medicinesBusiness.Update(model);
        }

        // DELETE api/<MedicinesController>/5
        [HttpDelete("{id}")]
        public async Task<Response<bool>> Delete(int id)
        => await _medicinesBusiness.Delete(id);
    }
}
