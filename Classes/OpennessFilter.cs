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
                        if (!ConnectingEntitiesAreOpen(line.Value, filtered.powerEntities))
                        {
                            toFilterOut.Add(line.Key);
                        }
                    }
                    break;
            }
            foreach (long key in toFilterOut) filtered.lines.Remove(key);
        }

        private bool ConnectingEntitiesAreOpen(LineEntity line, Dictionary<long, PowerEntity> entities)
        {
            if (entities.ContainsKey(line.FirstEnd) && entities[line.FirstEnd] is SwitchEntity && ((SwitchEntity)entities[line.FirstEnd]).Status == "Open")
            {
                return false;
            }

            if (entities.ContainsKey(line.SecondEnd) && entities[line.SecondEnd] is SwitchEntity && ((SwitchEntity)entities[line.SecondEnd]).Status == "Open")
            {
                return false;
            }

            return true;
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
