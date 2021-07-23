using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PZ2.Model
{
    public class PowerEntity
    {


        private double x;
        private double y;
        public string Name { get; set; }
        public long Id { get; set; }

        public double TranslatedX { get; set; }

        public double TranslatedY { get; set; }

        public int ConnectionCount { get; set; }



        public PowerEntity()
        {
            ConnectionCount = 0;
        }

        public override string ToString()
        {
            return string.Format($"{Name} Id:{Id}");
        }

        public Tuple<int,int> GridPosition  { get; set; }


        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public Image Image { get; set; }
    }
}
