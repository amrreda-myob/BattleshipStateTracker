using System.Collections.Generic;
using System.Linq;
using BattleshipStateTracker.Services.Models.Enums;

namespace BattleshipStateTracker.Services.Models
{
    public class Ship
    {
        public ShipDirection Direction { get; }
        public List<GridCell> Cells { get; }

        public Ship(ShipDirection direction)
        {
            Direction = direction;
            Cells = new List<GridCell>();
        }
        
        public bool IsSunk
        {
            get
            {
                return Cells.All(c => c.Status == GridCellStatus.Hit);
            }
        }
    }
}