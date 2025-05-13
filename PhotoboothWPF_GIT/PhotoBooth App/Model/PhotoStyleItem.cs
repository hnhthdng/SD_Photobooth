using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBooth_App.Model
{
    public class PhotoStyleItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imageUrl { get; set; }
        public string description { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
