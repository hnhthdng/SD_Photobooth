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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoBooth_App
{
    /// <summary>
    /// Interaction logic for Keyboard.xaml
    /// </summary>
    public partial class Keyboard : UserControl
    {
        public event Action<int> QuantitySelected;
        public event Action DeleteClicked;
        public event Action ChangeKeyboard;

        public Keyboard()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Content.ToString(), out int quantity))
            {
                QuantitySelected?.Invoke(quantity);
            }
        }
        private void Change_Click(object sender, RoutedEventArgs e)
        {
            ChangeKeyboard?.Invoke();
        }

        private void Delete_CLick(object sender, RoutedEventArgs e)
        {
            DeleteClicked?.Invoke();
        }
    }
}
