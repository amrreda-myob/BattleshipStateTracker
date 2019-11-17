using System;

namespace BattleshipStateTracker.Services.Models.Exceptions
{
    public class InvalidBattleInitiationException : Exception
    {
        public InvalidBattleInitiationException(string message) 
            : base(message)
        {
            
        }
    }
}