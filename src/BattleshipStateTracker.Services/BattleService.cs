using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleshipStateTracker.Services.Helpers;
using BattleshipStateTracker.Services.Models; 
using BattleshipStateTracker.Services.Models.Enums;
using BattleshipStateTracker.Services.Models.Exceptions;
using Microsoft.Extensions.Logging;

namespace BattleshipStateTracker.Services
{
    public class BattleService : IBattleService
    {
        private readonly ILogger<BattleService> _logger;
        private static IList<Battle> _battles;
        
        public BattleService(ILogger<BattleService> logger)
        {
            _logger = logger;
            _battles = new List<Battle>(); //TODO: should be replaces by Data Access Repository  
        }
        
        public async Task<Battle> InitiateBattle(int gridDimension, int numberOfShips, int shipLength)
        { 
            if(shipLength > gridDimension)
                throw new InvalidBattleInitiationException("Failed to initiate battle: ship length is greater than grid dimension.");
            
            var battle = new Battle(gridDimension, numberOfShips, shipLength);
            _battles.Add(battle);
            
            _logger.LogInformation($"battle initiated : {battle}.");
            
            return await Task.FromResult(battle);
        }

        public async Task<BattleStatus> GetBattleStatus(string battleId)
        {
            var battle = GetBattle(battleId);

            return await Task.FromResult(battle.Status);
        }
 
        public async Task<Ship> AddShip(string battleId, Coordinate coordinate, ShipDirection direction)
        {
            var battle = GetBattle(battleId);

            ValidateShipCreation(battle, coordinate, direction);

            var ship = await battle.AddShip(coordinate, direction);
 
            return ship;
        }

        public async Task<BattleResult> Attack(string battleId, Coordinate coordinate)
        {
            var battle = GetBattle(battleId);

            ValidateAttack(coordinate, battle);

            if (battle.Status == BattleStatus.Initialized) battle.Status = BattleStatus.InPlay;

            var attackedCell = battle.Grid.Cells[coordinate.Column, coordinate.Row];

            attackedCell.Status = attackedCell.Status switch
            {
                GridCellStatus.Empty => GridCellStatus.Miss,
                GridCellStatus.Battleship => GridCellStatus.Hit,
                _ => attackedCell.Status
            };

            _logger.LogInformation(
                $"Cell ({attackedCell.Coordinate.Column},{attackedCell.Coordinate.Row}) was attacked and the result is {Enum.GetName(typeof(GridCellStatus), attackedCell.Status)}");

            var allShipsSunk = battle.Grid.Ships.All(s => s.IsSunk);

            if (allShipsSunk)
            {
                battle.Status = BattleStatus.GameOver;
                _logger.LogInformation("Game Over! All ships has been sunk!");
            }

            return await Task.FromResult(new BattleResult
            {
                AttackedCellStatus = attackedCell.Status,
                AllShipsSunk = allShipsSunk,
                Status = battle.Status
            });
        }

        private void ValidateAttack(Coordinate coordinate, Battle battle)
        {
            if (battle.Status == BattleStatus.GameOver)
                throw new AttackFailedException("This battle is over. Can't process attack.");

            if (battle.Grid.Ships.Count != battle.Grid.NumberOfShips)
                throw new AttackFailedException(
                    $"You have to create a total of {battle.Grid.NumberOfShips} ships to start the battle");

            if (!battle.IsValidCoordinate(coordinate))
                throw new AttackFailedException(
                    $"The attacked cell is invalid column and row coordinates have to be from 0 to {battle.Grid.Dimension}");
        }

        private void ValidateShipCreation(Battle battle, Coordinate coordinate, ShipDirection direction)
        {
            if (battle.Status == BattleStatus.GameOver)
                throw new InvalidShipCreationException("This battle is over. Ship can't be created.");

            if (!battle.IsValidCoordinate(coordinate) || !battle.CanCreateShip(coordinate, direction))
                throw new InvalidShipCreationException("Ship can't be created.");

            if (battle.Grid.Ships.Count >= battle.Grid.NumberOfShips)
                throw new InvalidShipCreationException("Can't fit more ships");
        }

        private Battle GetBattle(string battleId)
        {
            if (!Guid.TryParse(battleId, out var id))
                throw new InvalidBattleIdException("Invalid battle id.");

            var battle = _battles.FirstOrDefault(b => b.Id == id);

            if (battle == null) throw new BattleIsNotExistException("The battle is not exist.");
            return battle;
        }
    }
}