using IMDbApiLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MovieSearch.Components.AdvancedSearchMVVM.Models;
using System.Collections.ObjectModel;

namespace MovieSearch.Components.AdvancedSearchMVVM.ViewModels
{

    public class SearchAdvancedViewModel:Screen
    {
        public ObservableCollection<Genres> Genres { get; set; }
        public ObservableCollection<Rating> Rating { get; set; }
        public ObservableCollection<Country> Country { get; set; }
        public ObservableCollection<TypeSearch> TypeSearch { get; set; }
        public ObservableCollection<Year> Year { get; set; }

        
        public SearchAdvancedViewModel() 
        {
            SearchModel model = new();
            Rating = new ObservableCollection<Rating>(model.GetRating());
            Genres = new ObservableCollection<Genres>(model.GetGenres());
            Country = new ObservableCollection<Country>(model.GetCountry());
            TypeSearch = new ObservableCollection<TypeSearch>(model.GetTypeSearches());
            Year = new ObservableCollection<Year>(model.GetReleaseYear());
        }
    }
}
