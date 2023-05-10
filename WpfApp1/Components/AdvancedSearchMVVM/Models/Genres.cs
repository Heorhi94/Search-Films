using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearch.Components.AdvancedSearchMVVM.Models
{
    public class Genres
    {
        public int GenresId { get; set; }
        public string Name { get; set; }

        public string GetItemGenres
        {
            get
            {
                return Name;
            }
        }
    }
}
