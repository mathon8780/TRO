using System.Collections.Generic;
using DataStructure;

namespace CoreComponents.DataRecord
{
    /// <summary>
    /// 行为消耗
    /// </summary>
    public class MovementCost
    {
        private Dictionary<E_MovementActions, MovementAndConsumption> _movementCosts;


        public MovementCost(List<MovementAndConsumption> movementAndConsumptions)
        {
            _movementCosts = new Dictionary<E_MovementActions, MovementAndConsumption>(movementAndConsumptions.Count);
            foreach (var mac in movementAndConsumptions)
            {
                _movementCosts.TryAdd(mac.movementActions, mac);
            }
        }

        public MovementAndConsumption GetMovementCost(E_MovementActions action)
        {
            return _movementCosts.GetValueOrDefault(action);
        }

        public float GetMovementSpeed(E_MovementActions action)
        {
            return _movementCosts.GetValueOrDefault(action).movementSpeed;
        }

        public bool CanTakeMove(E_MovementActions action, float currentEnergy)
        {
            var movementAndConsumption = _movementCosts.GetValueOrDefault(action);
            if (movementAndConsumption == null)
                return false;

            return currentEnergy > movementAndConsumption.minimumEnergyConstraint;
        }
    }
}