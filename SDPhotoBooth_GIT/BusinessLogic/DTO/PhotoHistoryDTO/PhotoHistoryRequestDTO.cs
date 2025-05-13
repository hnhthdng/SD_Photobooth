using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO.PhotoHistoryDTO
{
   public class PhotoHistoryRequestDTO
   {
        public string SessionCode { get; set; }
        public List<IFormFile> PhotoPaths { get; set; }
   }

    public class PhotoHistoryUrlRequestDTO
    {
        public string SessionCode { get; set; }
        public List<string> PhotoPaths { get; set; }
    }
}
