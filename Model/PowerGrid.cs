using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ2.Model
{
    public class PowerGrid
    {
        private int _width, _height;
        public PowerEntity[,] Grid { get; set; }

        public LineEntity[,] LineGrid { get; set; }
        public Dictionary<long, PowerEntity> PowerEntities { get; set; }
        public Dictionary<long, LineEntity> LineEntities { get; set; }

        public PowerGrid(int x, int y)
        {
            _width = x;
            _height = y;
            Grid = new PowerEntity[_width + 1, _height + 1];
            LineGrid = new LineEntity[_width + 1, _height + 1];
            PowerEntities = new Dictionary<long, PowerEntity>();
            LineEntities = new Dictionary<long, LineEntity>();
        }

        public bool IsPositionTaken(Tuple<int,int> position)
        {
            if (position.Item1 < 0 || position.Item2 < 0 || position.Item2 > _height || position.Item1 > _width)
            {
                throw new ArgumentException("Invalid position.");
            }
            return Grid[position.Item1, position.Item2] != null;
        }

        public bool IsLineTaken(Tuple<int, int> position)
        {
            if (position.Item1 < 0 || position.Item2 < 0 || position.Item2 > _height || position.Item1 > _width)
            {
                throw new ArgumentException("Invalid position.");
            }
            return LineGrid[position.Item1, position.Item2] != null;
        }

        public void AssignLine(LineEntity l)
        {
            /*
            if (PowerEntities.Values.Where(x => x.Id == l.FirstEnd).Count() > 0 && PowerEntities.Values.Where(x => x.Id == l.SecondEnd).Count() > 0
                && LineEntities.Values.Where(x => x.FirstEnd == l.FirstEnd && x.SecondEnd == l.SecondEnd).Count() == 0)
            {
                LineEntities.Add(l.Id, l);
            }
            */
            LineEntities.Add(l.Id, l);
        }

        private double GetLineLenght(LineEntity l)
        {
            PowerEntity start = PowerEntities[l.FirstEnd];
            PowerEntity end = PowerEntities[l.SecondEnd];

            double diffX = start.GridPosition.Item1 - end.GridPosition.Item1;
            double diffY = start.GridPosition.Item2 - end.GridPosition.Item2;

            double lenght = Math.Sqrt((diffX) * (diffX) + (diffY) * (diffY));

            return lenght;
        }

        internal void AddConnections(long entity)
        {
            if (PowerEntities.ContainsKey(entity)) PowerEntities[entity].ConnectionCount++;
        }

        public void  SortLines()
        {
            LineEntities = LineEntities.OrderBy(x => GetLineLenght(x.Value)).ToDictionary(x=>x.Key, x=>x.Value);
        }


        public List<Tuple<int, int>> AdjacentNodes(Tuple<int, int> position, bool firstPass, Tuple<int, int> target)
        {
            List<Tuple<int, int>> adjacent = new List<Tuple<int, int>>();

            Tuple<int, int> left = new Tuple<int, int>(position.Item1 - 1, position.Item2);
            if (left.Equals(target))
            {
                adjacent.Clear();
                adjacent.Add(left);
                return adjacent;
            }
            //if (position.Item1 > 0 && (!firstPass || (!_powerGrid.IsLineTaken(left) && !_powerGrid.IsPositionTaken(left))))
            if (PositionAllowed(left) && (!firstPass || (!IsLineTaken(left) && !IsPositionTaken(left))))
            {
                adjacent.Add(left);
            }

            Tuple<int, int> right = new Tuple<int, int>(position.Item1 + 1, position.Item2);
            if (right.Equals(target))
            {
                adjacent.Clear();
                adjacent.Add(right);
                return adjacent;
            }
            //if (position.Item1 < gridWidth && (!firstPass || (!_powerGrid.IsLineTaken(right) && !_powerGrid.IsPositionTaken(right))))
            if (PositionAllowed(right) && (!firstPass || (!IsLineTaken(right) && !IsPositionTaken(right))))
            {
                adjacent.Add(right);
            }

            Tuple<int, int> top = new Tuple<int, int>(position.Item1, position.Item2 - 1);
            if (top.Equals(target))
            {
                adjacent.Clear();
                adjacent.Add(top);
                return adjacent;
            }
            //if (position.Item2 > 0 && (!firstPass || (!_powerGrid.IsLineTaken(top) && !_powerGrid.IsPositionTaken(top))))
            if (PositionAllowed(top) && (!firstPass || (!IsLineTaken(top) && !IsPositionTaken(top))))
            {
                adjacent.Add(top);
            }

            Tuple<int, int> bottom = new Tuple<int, int>(position.Item1, position.Item2 + 1);
            if (bottom.Equals(target))
            {
                adjacent.Clear();
                adjacent.Add(bottom);
                return adjacent;
            }
            //if (position.Item2 < gridHeight && (!firstPass || (!_powerGrid.IsLineTaken(bottom) && !_powerGrid.IsPositionTaken(bottom))))
            if (PositionAllowed(bottom) && (!firstPass || (!IsLineTaken(bottom) && !IsPositionTaken(bottom))))
            {
                adjacent.Add(bottom);
            }

            return adjacent;
        }

        private bool PositionAllowed(Tuple<int, int> position)
        {
            if (position.Item1 < 0 || position.Item1 > _width || position.Item2 < 0 || position.Item2 > _height)
            {
                return false;
            }
            return true;
        }
    }
}
