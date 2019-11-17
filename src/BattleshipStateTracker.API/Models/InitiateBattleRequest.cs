namespace BattleshipStateTracker.API.Models
{
    public class InitiateBattleRequest
    {
        public int ShipLength { get; set; }
        public int Dimension { get; set; }
        public int NoOfShips { get; set; }
    }
}