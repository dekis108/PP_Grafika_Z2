using PZ2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PZ3.Classes
{
    public class ModelDisplayFilter
    {

        public ConnectionFilter connectionFilter { get; private set; }
        public OpennessFilter opennessFilter { get; private set; }
        public ResistanceFilter resistanceFilter { get; private set; }

        private DrawableElements unmodified;

        public ModelDisplayFilter(Model3DGroup map, Dictionary<long, PowerEntity> powerEntities, Dictionary<long, LineEntity> lineEntities)
        {
            unmodified = new DrawableElements(powerEntities, lineEntities);

            connectionFilter = new ConnectionFilter();
            opennessFilter = new OpennessFilter();
            resistanceFilter = new ResistanceFilter();
        }

        public DrawableElements FilterOut()
        {
            Dictionary<long, PowerEntity> filteredPowerEntities = new Dictionary<long, PowerEntity>();
            Dictionary<long, LineEntity> filteredLines = new Dictionary<long, LineEntity>();

            DrawableElements filtered = new DrawableElements();

            connectionFilter.ApplyFilter(unmodified, filtered);

            return filtered;
        }


        internal void SetConnectionFilter(string filter)
        {
            connectionFilter.SetFilter(filter);
        }
    }
}
