using PZ2.Model;
using PZ3.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PZ3.Classes
{
    public class ConnectionFilter : IModelFilter
    {
        public const string noFilter = "No Filter";
        public const string option1 = "0 - 3";
        public const string option2 = "3 - 5";
        public const string option3 = "6+";

        private string currentFilter;

        public ConnectionFilter()
        {
            currentFilter = noFilter;
        }

        public void ApplyFilter(DrawableElements filtered, DrawableElements unmodified)
        {
            List<long> toFilterOut = new List<long>();
            switch (currentFilter)
            {
                default:
                case noFilter:
                    return; //skip
                    break;
                case option1:
                    foreach (var entity in filtered.powerEntities)
                    {
                        if (entity.Value.ConnectionCount > 3) toFilterOut.Add(entity.Key);
                    }
                    break;
                case option2:
                    foreach (var entity in filtered.powerEntities)
                    {
                        if (!(entity.Value.ConnectionCount >= 3 && entity.Value.ConnectionCount <= 5)) toFilterOut.Add(entity.Key);
                    }
                    break;
                case option3:
                    foreach (var entity in filtered.powerEntities)
                    {
                        if (!(entity.Value.ConnectionCount > 5)) toFilterOut.Add(entity.Key);
                    }
                    break;
            }
            foreach (long key in toFilterOut) filtered.powerEntities.Remove(key);
        }

        public void SetFilter(string filter)
        {
            switch (filter)
            {
                default:
                case noFilter:
                    currentFilter = noFilter;
                    break;
                case option1:
                    currentFilter = option1;
                    break;
                case option2:
                    currentFilter = option2;
                    break;
                case option3:
                    currentFilter = option3;
                    break;
            }
        }

    }
}
