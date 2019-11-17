using System;

namespace BattleshipStateTracker.Services.Models.Exceptions
{
    public class FailedShipCreationException : Exception
    {
        public FailedShipCreationException(string message)
            : base(message)
        {
            
        }
    }
}