using System.Text.Json.Serialization;
using BattleshipStateTracker.Services.Models.Enums; 

namespace BattleshipStateTracker.Services.Models
{
    public class GridCell
    {
        public Coordinate Coordinate { get; } 

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GridCellStatus Status { get; set; }
        
        public GridCell(int row, int column)
        {
            Coordinate = new Coordinate(row, column);
            Status = GridCellStatus.Empty;
        }
    }
}