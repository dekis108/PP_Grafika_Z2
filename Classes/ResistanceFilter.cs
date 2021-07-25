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
            switch (currentFilter)
            {
                default:
                case noFilter:
                    foreach (var entity in all.lines) filtered.lines.Add(entity.Key, entity.Value);
                    break;
                case option1:
                    foreach (var entity in all.lines)
                    {
                        if (entity.Value.Resistance <= 1) filtered.lines.Add(entity.Key, entity.Value);
                    }
                    break;
                case option2:
                    foreach (var entity in all.lines)
                    {
                        if (entity.Value.Resistance >= 1 && entity.Value.Resistance <= 2) filtered.lines.Add(entity.Key, entity.Value);
                    }
                    break;
                case option3:
                    foreach (var entity in all.lines)
                    {
                        if (entity.Value.Resistance > 2) filtered.lines.Add(entity.Key, entity.Value);
                    }
                    break;
            }
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
