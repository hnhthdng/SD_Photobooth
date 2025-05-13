using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBooth_App.Model
{
    public class PhotoSelectedPageReturnData
    {
        public string ImagePath { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsEdited { get; set; } = false;
    }
}
