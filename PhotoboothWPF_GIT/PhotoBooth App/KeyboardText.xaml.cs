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
    /// Interaction logic for KeyboardText.xaml
    /// </summary>
    public partial class KeyboardText : UserControl
    {
        public event Action<String> TextSelected;
        public event Action DeleteClicked;
        public event Action ChangeKeyboard;
        public KeyboardText()
        {
            InitializeComponent();
        }
        private void Key_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                TextSelected?.Invoke(button.Content.ToString());
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