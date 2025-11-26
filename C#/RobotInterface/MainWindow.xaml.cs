using System.IO.Ports;
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
using ExtendedSerialPort_NS;

namespace RobotInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        bool toogle;
        ExtendedSerialPort serialPort1;
        public MainWindow()
        {
            InitializeComponent();
            serialPort1 = new ExtendedSerialPort("COM7", 115200, Parity.None, 8, StopBits.One);
            serialPort1.Open();

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void TextBoxEmission_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBoxréception.Text += ("Reçu : " + textBoxEmission.Text);
            
                textBoxEmission.Text = "";
            }
        }
        
        private void buttonEnvoyer_Click(object sender, RoutedEventArgs e)
        {
            
            TextBoxréception.Text += ("Reçu : "+textBoxEmission.Text + "\n");
                textBoxEmission.Text = "";
           
            if (toogle == false )
            {
                buttonEnvoyer.Background = Brushes.RoyalBlue;
                toogle = !toogle;
            }
            else
            {
                buttonEnvoyer.Background = Brushes.Beige;
                toogle = !toogle;

            }
           
        }
    }
}