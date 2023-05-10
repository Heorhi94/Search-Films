using IMDbApiLib;
using MovieSearch.Components.AdvancedSearchMVVM.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace MovieSearch
{
    /// <summary>
    /// Логика взаимодействия для AdvancedSearch.xaml
    /// </summary>
    public partial class AdvancedSearch : Window
    {
       

        public AdvancedSearch()
        {
            InitializeComponent();
            DataContext = new SearchAdvancedViewModel();
        }

      


        private void GenresBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
