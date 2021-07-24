using PZ2.Model;
using PZ3.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3.Classes
{
    public class ResistanceFilter : IModelFilter
    {
        public const string noFilter = "No Filter";
        public const string option1 = "0 - 1";
        public const string option2 = "1 - 2";
        public const string option3 = "2+";

        private string currentFilter;
        public ResistanceFilter()
        {
            currentFilter = noFilter;
        }

        public void ApplyFilter(DrawableElements all, DrawableElements filtered)
        {
            throw new NotImplementedException();
        }

        public void SetFilter(string filter)
        {
            throw new NotImplementedException();
        }
    }
}
