using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearch.Components.AdvancedSearchMVVM.Models
{
    public class TypeSearch
    {
        public string Name { get; set; }
        public int TypeId { get; set; }

        public string GetItemTypeSearch
        {
            get
            {
                return Name;
            }
        }
    }
}
