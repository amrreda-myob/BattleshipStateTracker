using System.Threading.Tasks;
using BattleshipStateTracker.Services.Models;
using BattleshipStateTracker.Services.Models.Enums;

namespace BattleshipStateTracker.Services.Helpers
{
    public static class BattleExtensions
    {
        public static async Task<Ship> AddShip(this Battle battle, Coordinate coordinate, ShipDirection direction)
        {
            var ship = new Ship(direction);
            
            if (direction == ShipDirection.Horizontal)
            {
                for (int i = coordinate.Column; i < coordinate.Column + battle.Grid.ShipLength; i++)
                {
                    battle.Grid.Cells[i, coordinate.Row].Status = GridCellStatus.Battleship;
                    ship.Cells.Add(battle.Grid.Cells[i, coordinate.Row]);
                }

                battle.Grid.Ships.Add(ship);
            }
            else
            {
                for (int j = coordinate.Row; j < coordinate.Row + battle.Grid.ShipLength; j++)
                {
                    battle.Grid.Cells[coordinate.Column, j].Status = GridCellStatus.Battleship;
                    ship.Cells.Add(battle.Grid.Cells[coordinate.Column, j]);
                }
                battle.Grid.Ships.Add(ship);
            }
            return await Task.FromResult(ship);
        }
        
        public static bool IsValidCoordinate(this Battle battle, Coordinate coordinate) =>
            coordinate.Column >= 0 
            && coordinate.Column < battle.Grid.Dimension 
            && coordinate.Row >= 0 
            && coordinate.Row < battle.Grid.Dimension;

        public static bool CanCreateShip(this Battle battle, Coordinate coordinate, ShipDirection direction)
        {
            return IsInsideGrid(battle.Grid, coordinate, direction) && CanAllocateCells(battle.Grid, coordinate, direction);
        }
        
        private static bool CanAllocateCells(Grid grid, Coordinate coordinate, ShipDirection direction)
        {
            if (direction == ShipDirection.Horizontal)
            {
                for (var i = coordinate.Column; i < coordinate.Column + grid.ShipLength; i++)
                    if (grid.Cells[i, coordinate.Row].Status != GridCellStatus.Empty)
                        return false;
            }
            else
            {
                for (var j = coordinate.Row; j < coordinate.Row + grid.ShipLength; j++)
                    if (grid.Cells[coordinate.Column, j].Status != GridCellStatus.Empty)
                        return false;
            }
            return true;
        }

        private static bool IsInsideGrid(Grid grid, Coordinate coordinate, ShipDirection direction)
        {
            if (direction == ShipDirection.Horizontal)
            {
                if ((coordinate.Column + grid.ShipLength) < grid.Dimension)
                    return true;
            }
            else
            {
                if ((coordinate.Row + grid.ShipLength) < grid.Dimension)
                    return true;
            }
            return false;
        }
    }
}