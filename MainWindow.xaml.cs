using Microsoft.Win32;
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

namespace Youtube_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        OpenFileDialog od = new OpenFileDialog();
        
        public static string Path = "";
        public static string APIKEY = "";
        //

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            od.Filter = "JSON|*.json";
            od.FileName = "Choose Watch history JSON";
            od.Multiselect = false;

            Nullable<bool> result = od.ShowDialog();

            if(result == true)
            {
                History.Text = od.FileName;
                Path = od.FileName;
            }

            
        }

        private async void Execute_ClickAsync(object sender, RoutedEventArgs e)
        {
            if( Api_Key.Text == "" || History.Text == "")
            {
                MessageBox.Show("Kindly Fill all the INPUT", "INFO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            APIKEY = Api_Key.Text;

            //Call Google API
            Video_data CT = new Video_data();
            
            await CT.Catogery_Data();
            await CT.Video_Metadata(Path, ProgressBar1, EntryLabel);

            //Call HTTP Method
            //Catogries CTS = new Catogries();
            //await CTS.GetCatogries(APIKEY);
            //VideoDetails VD = new VideoDetails();
            //await VD.GetVideo(APIKEY);
              
            
            MessageBox.Show("Task Completed", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);

        }

    }
}
