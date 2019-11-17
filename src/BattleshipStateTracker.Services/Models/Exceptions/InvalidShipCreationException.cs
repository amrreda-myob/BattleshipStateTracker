using System;

namespace BattleshipStateTracker.Services.Models.Exceptions
{
    public class InvalidShipCreationException : Exception
    {
        public InvalidShipCreationException(string message)
            : base(message)
        {
            
        }
    }
}