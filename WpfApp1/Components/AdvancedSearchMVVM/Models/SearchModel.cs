using Caliburn.Micro;
using IMDbApiLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Schema;

namespace MovieSearch.Components.AdvancedSearchMVVM.Models
{
    public class SearchModel
    {
        readonly List<string> genres = new()
        {
            "Action",
            "Adventure",
            "Comedy",
            "Drama",
            "Romance",
            "Thriller",
            "Horror",
            "Science Fiction",
            "Fantasy",
            "Animation",
            "Crime",
            "Mystery",
            "Family",
            "History",
            "War"
        };
        readonly List<string> typeSearch = new()
        {
            "Movies",
            "TV Shows",
            "Animated Movies",
            "Documentaries",
            "TV Series",
            "Anime",
            "Short Films"
        };
        readonly List<string> country = new()
        {
            "USA",
            "UK",
            "Canada",
            "Australia",
            "Germany",
            "France",
            "Italy",
            "Japan",
        };

//Add item releaseYearBox
        private List<string> AddItemYears()
        {
            var years = new List<string>();
            for (int i = 2000; i <= DateTime.Now.Year; i++)
            {
                years.Add(i.ToString());
            }
            return years;
        }
        Year GetItemYear(int id)
        {
            var release = new Year
            {
                YearId = id + 1,
                Years = AddItemYears()[id]
            };
            return release;
        }
        public List<Year> GetReleaseYear()
        {
            int total = AddItemRating().Count;
            var years = new List<Year>();
            for(int i = 0; i < total; i++)
            {
                years.Add(GetItemYear(i));
            }
            return years;
        }

// Add item boxRating      
        Rating GetItemRating(int id)
        {
            var rat = new Rating { RatingId = id + 1, Name = AddItemRating()[id] };
            return rat;
        }
        private List<string> AddItemRating()
        {
            var rating = new List<string>();    
            double minRating = 1.0;
            double maxRating = 10.0;
            double step = 0.5;

            for (double value = minRating; value <= maxRating; value += step)
            {
                rating.Add($"{value}⭐");
            }
            return rating;
        }
        public List<Rating> GetRating()
        {
            int total = AddItemRating().Count;
           List<Rating> rating = new();
            for(int i = 0; i < total; i++)
            {
                rating.Add(GetItemRating(i));
            }
            return rating;
        }

//Add item CountryBox
        Country GetItemCountry(int id) 
        {
            var countr = new Country
            {
                CountryId = id + 1,
                Name = country[id]
            };
            return countr;
        }

        public List<Country> GetCountry()
        {
            int total = country.Count;
            var result = new List<Country>();
            for(int i = 0; i < total; i++)
            {
                result.Add(GetItemCountry(i));
            }
            return result;
        }

//Add item TypeSearchBox
    TypeSearch GetItemTypeSearch(int id)
        {
            var type = new TypeSearch { TypeId = id + 1, Name = typeSearch[id] };
            return type;
        }
        public List<TypeSearch> GetTypeSearches()
        {
            int total = typeSearch.Count;
            var result = new List<TypeSearch>();
            for(var i = 0; i < total; i++)
            {
                result.Add(GetItemTypeSearch(i));
            }
            return result;
        }

//Add item Genres
    Genres GetItemGenres(int id)
        {
            var result = new Genres { GenresId = id + 1, Name = genres[id] };
            return result;
        }
        public List <Genres> GetGenres() 
        {
            int total = genres.Count;
            var result = new List<Genres>();
            for(int i = 0; i < total; i++)
            {
                result.Add(GetItemGenres(i));
            }
            return result;
        }
    }
}
