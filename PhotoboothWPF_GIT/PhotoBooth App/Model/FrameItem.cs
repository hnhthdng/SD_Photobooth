using PhotoBooth_App.Model;
using PhotoBooth_App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;

namespace PhotoBooth_App.Model
{
    public class FrameItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FrameUrl { get; set; }
        public string Description { get; set; }
        public int SlotCount { get; set; }
        public List<CoordinateItem> Coordinates { get; set; }
    }

    public class CoordinateItem
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

}

