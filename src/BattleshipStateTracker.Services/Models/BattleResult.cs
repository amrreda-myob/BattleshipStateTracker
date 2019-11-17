using BattleshipStateTracker.Services.Models.Enums;

namespace BattleshipStateTracker.Services.Models
{
    public class BattleResult
    {
        public GridCellStatus AttackedCellStatus { get; set; }
        public bool AllShipsSunk { get; set; }
        public BattleStatus Status { get; set; }
    }
}