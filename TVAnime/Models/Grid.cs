using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVAnime.Models
{
    struct Grid : IEquatable<Grid>
    {
        public int row;
        public int column;

        public Grid(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public override bool Equals(object obj)
        {
            return obj is Grid grid && Equals(grid);
        }

        public bool Equals(Grid other)
        {
            return row == other.row &&
                   column == other.column;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(row, column);
        }
        public static bool operator ==(Grid c1, Grid c2)
        {
            return c1.Equals(c2);
        }
        public static bool operator !=(Grid c1, Grid c2)
        {
            return !c1.Equals(c2);
        }
    }
}
