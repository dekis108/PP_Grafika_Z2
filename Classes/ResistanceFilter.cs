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
                    foreach (var line in filtered.lines)
                    {
                        if (line.Value.Resistance > 1) toFilterOut.Add(line.Key);
                    }
                    break;
                case option2:
                    foreach (var line in filtered.lines)
                    {
                        if (!(line.Value.Resistance >= 1 && line.Value.Resistance <= 2)) toFilterOut.Add(line.Key);
                    }
                    break;
                case option3:
                    foreach (var line in filtered.lines)
                    {
                        if (line.Value.Resistance <= 2) toFilterOut.Add(line.Key);
                    }
                    break;
            }
            foreach (long key in toFilterOut) filtered.lines.Remove(key);
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
