using PZ2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3.Classes
{
    public class DrawableElements
    {
        public Dictionary<long, PowerEntity> powerEntities;
        public Dictionary<long, LineEntity> lines;

        public DrawableElements(Dictionary<long, PowerEntity> entities, Dictionary<long, LineEntity> lines)
        {
            powerEntities = entities;
            this.lines = lines;
        }

        public DrawableElements()
        {
            powerEntities = new Dictionary<long, PowerEntity>();
            lines = new Dictionary<long, LineEntity>();
        }

    }
}
