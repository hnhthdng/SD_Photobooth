using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhotoBooth_App
{
    /// <summary>
    /// Interaction logic for ListPhotoEdited.xaml
    /// </summary>
    public partial class ListPhotoEdited : Window
    {
        public ListPhotoEdited(List<BitmapImage> processedImages)
        {
            InitializeComponent();
            ImagesList.ItemsSource = processedImages;
        }
    }

}
