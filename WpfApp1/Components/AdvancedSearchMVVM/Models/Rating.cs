using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearch.Components.AdvancedSearchMVVM.Models
{
    public class Rating
    {
        public int RatingId { get; set; }
        public string Name { get; set; }

        public string GetItemRating
        {
            get
            {
                return Name;
            }
        }
    }
}
