using IMDbApiLib;
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

namespace Movies
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

        private async void buttom_Click(object sender, RoutedEventArgs e)
        {
            var api = new ApiLib("k_y76947gh");
            var result = await api.SearchMovieAsync(searc.Text);

            list.ItemsSource = result.Results;
            list.DisplayMemberPath = "Title";
            list.SelectedValuePath = "Id";
        }
    }
}
