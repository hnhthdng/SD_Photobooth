using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBooth_App.Model
{
    public class ResponseRabbitMQDTO
    {
        public string CorrelationId { get; set; }
        public string[] ProcessedImageUrl { get; set; }
    }
}
