using PZ2.Model;
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
        public const string noFilter = "No Filter";
        public const string option1 = "Only closed";

        public OpennessFilter()
        {
            currentFilter = noFilter;
        }

        public void ApplyFilter(DrawableElements filtered)
        {
            //TODO
            List<long> toFilterOut = new List<long>();
            switch (currentFilter)
            {
                default:
                case noFilter:
                    foreach (var line in filtered.lines) filtered.lines.Add(line.Key, line.Value);
                    break;
                case option1:
                    foreach (var line in filtered.lines)
                    {
                        if (ConnectingEntitiesAreOpen(line))
                        {
                            filtered.lines.Add(line.Key, line.Value);
                        }
                    }
                    break;
            }
            foreach (long key in toFilterOut) filtered.powerEntities.Remove(key);
        }

        private bool ConnectingEntitiesAreOpen(KeyValuePair<long, LineEntity> line)
        {
            //TODO: if levi ili desni je switch i to closed -> izbaci
            throw new NotImplementedException();
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
            }
        }
    }
}
