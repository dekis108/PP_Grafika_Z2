using PZ2.Model;
using PZ3.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3.Interfaces
{
    interface IModelFilter
    {
        void ApplyFilter(DrawableElements filtered, DrawableElements unmodified);

        void SetFilter(string filter);
    }
}
