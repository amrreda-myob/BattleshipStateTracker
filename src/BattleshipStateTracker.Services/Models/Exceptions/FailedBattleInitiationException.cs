using System;

namespace BattleshipStateTracker.Services.Models.Exceptions
{
    public class FailedBattleInitiationException : Exception
    {
        public FailedBattleInitiationException(string message)
            : base(message)
        {
            
        }
    }
}