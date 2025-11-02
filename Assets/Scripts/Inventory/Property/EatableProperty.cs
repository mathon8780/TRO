using System.Collections.Generic;
using Inventory.Save;
using UnityEngine;

namespace Inventory.Property
{
    public class EatableProperty : ItemProperty, ISerializableProperty
    {
        public float Cab;
        public float Fat;
        public float Protein;
        public float Water;
        public float Energy;


        public EatableProperty(float cab, float fat, float protein, float water, float energy)
        {
            Cab = cab;
            Fat = fat;
            Protein = protein;
            Water = water;
            Energy = energy;
        }


        #region ISerializableProperty

        public void SetPropertyData(string propertyName, object propertyValue)
        {
            var prop = GetType().GetProperty(propertyName);
            if (prop != null)
            {
                prop.SetValue(this, propertyValue);
                return;
            }

            var field = this.GetType().GetField(propertyName);
            if (field != null)
            {
                field.SetValue(this, propertyValue);
                return;
            }

            Debug.LogWarning($"property {propertyName} not found");
        }

        public Dictionary<string, object> SerializeState()
        {
            var state = new Dictionary<string, object>()
            {
                [nameof(Cab)] = Cab,
                [nameof(Fat)] = Fat,
                [nameof(Protein)] = Protein,
                [nameof(Water)] = Water,
                [nameof(Energy)] = Energy
            };
            return state;
        }

        public void DeserializeState(Dictionary<string, object> serializedData)
        {
            foreach (var pk in serializedData)
            {
                SetPropertyData(pk.Key, pk.Value);
            }
        }

        #endregion
    }
}