using PZ3.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3.Classes
{
    public class OpennessFilter : IModelFilter
    {
        private string currentFilter;

        public OpennessFilter()
        {

        }

        public void ApplyFilter(DrawableElements all, DrawableElements filtered)
        {
            
        }

        public void SetFilter(string filter)
        {
            throw new NotImplementedException();
        }
    }
}
