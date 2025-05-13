using PhotoBooth_App.Handler;
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
    /// Interaction logic for TitleControl.xaml
    /// </summary>
    public partial class TitleControl : UserControl
    {
        public TitleControl()
        {
            InitializeComponent();
            LanguageHandler.LanguageChanged += (s, e) => UpdateTitle();
        }

        public static readonly DependencyProperty TitleKeyProperty =
            DependencyProperty.Register("TitleKey", typeof(string), typeof(TitleControl),
                new PropertyMetadata(string.Empty, OnTitleKeyChanged));

        public string TitleKey
        {
            get => (string)GetValue(TitleKeyProperty);
            set => SetValue(TitleKeyProperty, value);
        }

        private static void OnTitleKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TitleControl control)
            {
                control.UpdateTitle();
            }
        }

        public void UpdateTitle()
        {
            if (!string.IsNullOrEmpty(TitleKey) && Application.Current.Resources.Contains(TitleKey))
            {
                TitleText.Text = Application.Current.Resources[TitleKey] as string;
            }
        }
    }
}