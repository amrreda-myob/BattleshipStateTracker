using System;

namespace BattleshipStateTracker.Services.Models.Exceptions
{
    public class InvalidBattleIdException : Exception
    {
        public InvalidBattleIdException(string message) 
            : base(message)
        {
            
        }
    }
}