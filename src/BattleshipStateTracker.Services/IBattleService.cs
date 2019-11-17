using System.Threading.Tasks;
using BattleshipStateTracker.Services.Models;
using BattleshipStateTracker.Services.Models.Enums;

namespace BattleshipStateTracker.Services
{
    public interface IBattleService
    {
        Task<Battle> InitiateBattle(int gridDimension, int numberOfShips, int shipLength);
        Task<BattleStatus> GetBattleStatus(string battleId);
        Task<Ship> AddShip(string battleId, Coordinate coordinate, ShipDirection direction);
        Task<BattleResult> Attack(string battleId, Coordinate attackCoordinate);
    }
}