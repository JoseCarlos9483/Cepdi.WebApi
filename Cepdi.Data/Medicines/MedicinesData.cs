using Cepdi.Models.Enum;
using Cepdi.Models.Interfaces.Medicines;
using Cepdi.Models.Models.Medicines;
using Cepdi.Tools.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cepdi.Data.Medicines
{
    public class MedicinesData : IMedicinesData
    {
        private readonly IConfiguration _configuration;

        public MedicinesData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> Create(CreateMedicineModel model)
        {
            List<MedicineModel> listRestul = GetListMedicine();

            var newId = (listRestul.Last().IIDMedicamento) + 1;

            string newRegister = $"{newId}|{model.Nombre}|{model.Concentracion}|{model.IdFarmaceutica}|{model.Precio}|{model.Stock}|{model.Presentacion}|{model.Habilitado}";

            using (StreamWriter sw = File.AppendText(_configuration["FilePaths:Medicines"]))
            {
                sw.WriteLine(newRegister);
            }
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            bool success = false;

            List<MedicineModel> listRestul = GetListMedicine();


            if (listRestul.Where(i => i.IIDMedicamento == id).FirstOrDefault() != null)
            {
                listRestul.Where(i => i.IIDMedicamento == id).FirstOrDefault().BHabilitado = (int)Status.disabled;

                WriteFile(listRestul);

                success = true;
            }
            

            return success;
        }

        public async Task<MedicineModel> Get(int id)
        {
            List<MedicineModel> listRestul = GetListMedicine();

            return listRestul.Where(i => i.IIDMedicamento == id ).FirstOrDefault();

                
        }

        public async Task<(int totalRecord, IEnumerable<MedicineModel> registers)> GetAll(ShowTableMedicinesModel model)
        {

            List<MedicineModel> listRestul = GetListMedicine().Where(x => x.BHabilitado == (int)Status.enabled).ToList();          


            var countRegisters = listRestul.Count();             

            var getTotalRegisters = model.Currentpage * model.RecordPorPage;             

            var position = 0;

            if (model.Currentpage > 1) {
                model.Currentpage--;
                position = (model.Currentpage * model.RecordPorPage);
            }         
            if (getTotalRegisters > countRegisters)
            {             

                listRestul = listRestul.GetRange(position, (countRegisters - position));
            }
            else {
                 listRestul = listRestul.GetRange(position, model.RecordPorPage);
            }

            return  (countRegisters, listRestul);
        }

        public async Task<bool> Update(MedicineModel model)
        {
            bool success = false;

            List<MedicineModel> listRestul = GetListMedicine();

            var updateModel = listRestul.Where(i => i.IIDMedicamento == model.IIDMedicamento).FirstOrDefault();

            if (updateModel != null) {
                updateModel.Nombre = model.Nombre;
                updateModel.Concentracion = model.Concentracion;
                updateModel.IIdFarmaFarmaceutica = model.IIdFarmaFarmaceutica;
                updateModel.Precio = model.Precio;
                updateModel.Stock = model.Stock;
                updateModel.Presentacion = model.Presentacion;
                updateModel.BHabilitado = model.BHabilitado;

                var objectRemove = listRestul.Where(x => x.IIDMedicamento == model.IIDMedicamento).First();
                
                listRestul.Remove(objectRemove);

                listRestul.Add(updateModel);
                listRestul.OrderBy(x => x.IIdFarmaFarmaceutica);

                WriteFile(listRestul);

                success = true;
            }

            
            return success;
        }

        private List<MedicineModel> GetListMedicine() {

            List<MedicineModel> listRestul = new List<MedicineModel>();
            var lines = Helpers.OpenFile(_configuration["FilePaths:Medicines"]);

            foreach (var line in lines)
            {
                string[] words = line.Split('|');
                MedicineModel medicine = new()
                {
                    IIDMedicamento = Convert.ToInt32(words[0]),
                    Nombre = words[1],
                    Concentracion = words[2],
                    IIdFarmaFarmaceutica = Convert.ToInt32(words[3]),
                    Precio = Convert.ToDecimal(words[4]),
                    Stock = Convert.ToInt32(words[5]),
                    Presentacion = words[6],
                    BHabilitado = Convert.ToByte(words[7])
                };

                listRestul.Add(medicine);
            }

            return listRestul;
        }

        private void WriteFile(IEnumerable<MedicineModel> listRestul) {

            using (StreamWriter writer = new StreamWriter(_configuration["FilePaths:Medicines"], false))
            {
                string header = "IIDMEDICAMENTO|NOMBRE|CONCENTRACION|IIDFORMAFARMACEUTICA|PRECIO|STOCK|PRESENTACION|BHABILITADO";

                writer.WriteLine(header);

                foreach (var member in listRestul)
                {

                    string value = $"{member.IIDMedicamento}|{member.Nombre}|{member.Concentracion}" +
                        $"|{member.IIdFarmaFarmaceutica}|{member.Precio}|{member.Stock}" +
                        $"|{member.Presentacion}|{member.BHabilitado}";

                    writer.WriteLine(value);
                }

            }
        }

    }
}
