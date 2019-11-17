using System.Collections.Generic;
using Newtonsoft.Json;

namespace BattleshipStateTracker.Services.Models
{
    public class Grid
    { 
        [JsonIgnore]
        public GridCell[,] Cells { get; }
        public int Dimension { get; }
        public int NumberOfShips { get; set; }
        public int ShipLength { get; }
        public IList<Ship> Ships { get; set; }

        public Grid(int dimension, int numberOfShips, int shipLength)
        {
            Dimension = dimension;
            NumberOfShips = numberOfShips;
            ShipLength = shipLength;
            Ships = new List<Ship>();
            
            Cells = new GridCell[dimension,dimension];
            
            for (var i = 0; i < dimension; i++)
            for (var j = 0; j < dimension; j++)
            {
                Cells[i, j] = new GridCell(i, j);
            }
        }
    }
}