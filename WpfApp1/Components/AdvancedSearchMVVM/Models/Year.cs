using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MovieSearch.Components.AdvancedSearchMVVM.Models
{
    public class Year
    {
        public int YearId { get; set; }
        public string Years { get; set; }

        public string GetItemReleaseYear
        {
            get
            {
                return Years;
            }
        }
    }
}
