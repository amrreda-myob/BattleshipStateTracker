using System;

namespace BattleshipStateTracker.Services.Models.Exceptions
{
    public class AttackFailedException : Exception
    {
        public AttackFailedException(string message)
        : base(message)
        {
            
        }
    }
}