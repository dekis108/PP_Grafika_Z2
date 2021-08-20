using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PZ2.Model
{

    public enum ConductorMaterial { Other = 0, Steel, Acsr, Copper }

    public class LineEntity
    {
        private long id;
        private long firstEnd;
        private long secondEnd;
        public string Name { get; set; }

        public List<Point> Vertices { get; set; }

        public bool Assigned { get; set; }

        public ConductorMaterial ConductorMaterial { get; set; }

        public double Resistance { get; set; }

        public override string ToString()
        {
            return string.Format($"{Name} Id:{Id}\nStart:{firstEnd}\nEnd:{secondEnd}");
        }

        public LineEntity()
        {
            Vertices = new List<Point>();
        }

        public long Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }



        public long FirstEnd
        {
            get
            {
                return firstEnd;
            }

            set
            {
                firstEnd = value;
            }
        }

        public long SecondEnd
        {
            get
            {
                return secondEnd;
            }

            set
            {
                secondEnd = value;
            }
        }

        public Polyline Polyline { get; set; }

        internal void SelectLine()
        {
            Polyline.Stroke = Brushes.Blue;
        }
    }
}
