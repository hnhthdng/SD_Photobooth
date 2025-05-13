using BusinessLogic.DTO.ExportProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAPI.Services.IService
{
    public interface IExportService
    {
        byte[] ExportProductsToCsv(IEnumerable<ExportProductResponseDTO> products);
    }
}
