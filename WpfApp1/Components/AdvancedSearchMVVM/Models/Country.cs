using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearch.Components.AdvancedSearchMVVM.Models
{
    public class Country
    {
        public string Name { get; set; }
        public int CountryId { get; set; }

        public string GetItemCountry
        {
            get
            {
                return Name;
            }
        }

    }
}
