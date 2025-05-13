using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO.RabbitMQDTO
{
    public class ResponseRabbitMQDTO
    {
        public string CorrelationId { get; set; }
        public string[] ProcessedImageUrl { get; set; }
        public string SessionCode { get; set; }
        public int PhotoStyleId { get; set; }
    }
}
