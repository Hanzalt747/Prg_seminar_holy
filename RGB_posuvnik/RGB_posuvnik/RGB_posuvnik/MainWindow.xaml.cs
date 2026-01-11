using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RGB_posuvnik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Red_value.Text = "0";
            Green_value.Text = "0";
            Blue_value.Text = "0";
           
            UpdateColor();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateColor();
        }

        private void UpdateColor()
        {
            byte r = (byte)Red_slider.Value;
            byte g = (byte)Green_slider.Value;
            byte b = (byte)Blue_slider.Value;

            Red_value.Text = r.ToString();
            Green_value.Text = g.ToString();
            Blue_value.Text = b.ToString();

            var color = Color.FromRgb(r, g, b);
            RGB_shower.Fill = new SolidColorBrush(color);

            // Show hex color
            Color_value.Content = $"#{r:X2}{g:X2}{b:X2}";
        }

        private void Red_value_TextChanged(object sender, TextChangedEventArgs e)
        {
            byte.TryParse(Red_value.Text, out byte value);
            Red_slider.Value=value;
        }

        private void Blue_value_TextChanged(object sender, TextChangedEventArgs e)
        {
            byte.TryParse(Blue_value.Text, out byte value);
            Blue_slider.Value = value;
        }

        private void Green_value_TextChanged(object sender, TextChangedEventArgs e)
        {
            byte.TryParse(Green_value.Text, out byte value);
            Green_slider.Value = value;
        }
    }
}