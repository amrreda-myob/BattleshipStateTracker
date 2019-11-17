using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BattleshipStateTracker.Services;
using BattleshipStateTracker.Services.Models;
using BattleshipStateTracker.Services.Models.Enums;
using BattleshipStateTracker.Services.Models.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BattleStateTracker.Unit.Tests
{
    public class BattleServiceTest
    {
        private readonly BattleService _battleService;
        private Mock<ILogger<BattleService>> _logger;

        public BattleServiceTest()
        {
            _logger = new Mock<ILogger<BattleService>>();
            
            _battleService = new BattleService(_logger.Object);
        }

        [Fact] 
        public async Task ShouldInitiateBattle_Create_Battle_Successfully_When_Pass_ValidInputs()
        { 
            var battle = await _battleService.InitiateBattle(10, 6, 3);
            
            Assert.NotEqual(Guid.Empty, battle.Id);
            Assert.Equal(BattleStatus.Initialized, battle.Status);
            Assert.Equal(100, battle.Grid.Cells.Length);
            Assert.Equal(GridCellStatus.Empty, battle.Grid.Cells[9, 9].Status);
            Assert.Equal(9, battle.Grid.Cells[9, 9].Coordinate.Column);
        }

        [Fact]
        public async Task ShouldInitiateBattle_Throw_Exception_When_Given_ShipLength_Greater_Than_Given_GridDimension()
        {
            await Assert.ThrowsAsync<InvalidBattleInitiationException>(() => 
                _battleService.InitiateBattle(5, 6, 6));
        }
        
        [Fact] 
        public async Task ShouldGetBattleStatus_Return_Battle_Status_Successfully_When_Pass_ValidBattleId()
        { 
            var battle1 = await _battleService.InitiateBattle(10, 6, 3);
            var battle2 = await _battleService.InitiateBattle(5, 6, 3);
 
            var battleStatus1 = await _battleService.GetBattleStatus(battle1.Id.ToString());
            var battleStatus2 = await _battleService.GetBattleStatus(battle2.Id.ToString());
             
            Assert.Equal(BattleStatus.Initialized, battleStatus1);
            Assert.Equal(BattleStatus.Initialized, battleStatus2); 
        }
        
        [Fact] 
        public async Task ShouldGetBattleStatus_Throw_Exception_When_Pass_InvalidBattleId()
        {  
            await Assert.ThrowsAsync<InvalidBattleIdException>(() =>
                _battleService.GetBattleStatus("invalid-guid"));  
        }
        
        [Fact] 
        public async Task ShouldGetBattleStatus_Throw_Exception_When_Battle_Is_Not_Exist()
        {  
            await Assert.ThrowsAsync<BattleIsNotExistException>(() =>
                _battleService.GetBattleStatus(Guid.NewGuid().ToString()));  
        }
        
        [Fact] 
        public async Task ShouldAddShip_Return_Ship_When_Created_Successfully()
        {  
            var battle = await _battleService.InitiateBattle(10, 6, 3);

            var ship = await _battleService.AddShip(battle.Id.ToString(), new Coordinate(0, 0),
                ShipDirection.Horizontal);
            
            Assert.NotNull(ship);
            Assert.Contains(ship.Cells, (c) => c.Status == GridCellStatus.Battleship);
            Assert.Equal(ShipDirection.Horizontal, ship.Direction);
        }
        
        [Fact] 
        public async Task ShouldAddShip_Throw_Exception_When_Game_Is_Over()
        {  
            var battle = await _battleService.InitiateBattle(10, 6, 3);
            battle.Status = BattleStatus.GameOver;
            
            await Assert.ThrowsAsync<InvalidShipCreationException>(() =>  _battleService.AddShip(battle.Id.ToString(), new Coordinate(0, 0),
                ShipDirection.Horizontal));
        }
        
        [Fact] 
        public async Task ShouldAddShip_Throw_Exception_When_Given_Coordinates_Is_Invalid()
        {  
            var battle = await _battleService.InitiateBattle(10, 6, 3);
           
            await Assert.ThrowsAsync<InvalidShipCreationException>(() =>  _battleService.AddShip(battle.Id.ToString(), new Coordinate(11, 11),
                ShipDirection.Horizontal));
        }
        
        [Fact] 
        public async Task ShouldAddShip_Throw_Exception_When_Given_No_Of_Ships_Exceeeded()
        {  
            var battle = await _battleService.InitiateBattle(10, 2, 3);
            battle.Grid.Ships = new List<Ship>()
            {
                new Ship(ShipDirection.Horizontal),
                new Ship(ShipDirection.Horizontal)
            };
            
            await Assert.ThrowsAsync<InvalidShipCreationException>(() =>  _battleService.AddShip(battle.Id.ToString(), new Coordinate(11, 11),
                ShipDirection.Horizontal));
        }
        
        [Fact] 
        public async Task ShouldAttack_Return_Battle_Result_When_Given_Valid_Coordinates_Successfully()
        {  
            var battle = await _battleService.InitiateBattle(10, 2, 3);

            var battleId = battle.Id.ToString();

            var shipCoordinate = new Coordinate(0, 0);
            
            await _battleService.AddShip(battleId, shipCoordinate,
                ShipDirection.Horizontal);
            await _battleService.AddShip(battleId, new Coordinate(1, 0), 
                ShipDirection.Horizontal);

            var result = await _battleService.Attack(battleId, shipCoordinate);
            
            Assert.Equal(GridCellStatus.Hit ,result.AttackedCellStatus);
            Assert.Equal(BattleStatus.InPlay ,result.Status);
            Assert.False(result.AllShipsSunk);
        }

        [Fact]
        public async Task ShouldAttack_Throw_Exception_When_Ship_Created_Is_Less_Than_NumberOfShips()
        {
            var battle = await _battleService.InitiateBattle(10, 2, 3);

            var battleId = battle.Id.ToString();

            var shipCoordinate = new Coordinate(0, 0);

            await _battleService.AddShip(battleId, shipCoordinate,
                ShipDirection.Horizontal);


            await Assert.ThrowsAsync<AttackFailedException>(
                () => _battleService.Attack(battleId, shipCoordinate));
        }
        
        [Fact] 
        public async Task ShouldAttack_Throw_Exception_When_Game_Is_Over()
        {  
            var battle = await _battleService.InitiateBattle(10, 6, 3);
            
            var battleId = battle.Id.ToString();

            var shipCoordinate = new Coordinate(0, 0);

            await _battleService.AddShip(battleId, shipCoordinate,
                ShipDirection.Horizontal);
            
            await Assert.ThrowsAsync<AttackFailedException>(
                () => _battleService.Attack(battleId, shipCoordinate));
        }
        
        [Fact]
        public async Task ShouldAttack_Throw_Exception_When_Given_Coordinate_Invalid()
        {
            var battle = await _battleService.InitiateBattle(10, 2, 3);

            var battleId = battle.Id.ToString();

            var shipCoordinate = new Coordinate(0, 0);
            var attackCoordinate = new Coordinate(10, 10);

            await _battleService.AddShip(battleId, shipCoordinate,
                ShipDirection.Horizontal);
 
            await Assert.ThrowsAsync<AttackFailedException>(
                () => _battleService.Attack(battleId, attackCoordinate));
        }

        [Fact] public async Task ShouldAttack_Return_Battle_Status_GameOver_When_AllShipsSunk()
        {  
            var battle = await _battleService.InitiateBattle(10, 2, 2);

            var battleId = battle.Id.ToString();

            
            await _battleService.AddShip(battleId, new Coordinate(0, 0),
                ShipDirection.Horizontal);
            await _battleService.AddShip(battleId, new Coordinate(1, 0), 
                ShipDirection.Horizontal);

            await _battleService.Attack(battleId, new Coordinate(0, 0));
            await _battleService.Attack(battleId, new Coordinate(0, 1));
            
            
            await _battleService.Attack(battleId, new Coordinate(1, 0));
            var result = await _battleService.Attack(battleId, new Coordinate(1, 1));

             
            Assert.True(result.AllShipsSunk);
        }
    }
}