namespace BattleshipStateTracker.API.Models
{
    public class AddShipRequest
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public string ShipDirection { get; set; }
    }
}