using System;
using System.Collections.Generic;
using Inventory.PropertyData;
using Inventory.Save;
using UnityEngine;

namespace Inventory.Property
{
    public class ItemContainerProperty : ItemProperty, ISerializableProperty
    {
        // Data数据
        public float Capacity;
        public float EncumbranceReduceRate;

        // 动态数据
        public List<Item> ContainerContent;

        public ItemContainerProperty(float capacity, float encumbranceReduceRate)
        {
            Capacity = capacity;
            EncumbranceReduceRate = encumbranceReduceRate;
            ContainerContent = new List<Item>();
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
                [nameof(Capacity)] = Capacity,
                [nameof(EncumbranceReduceRate)] = EncumbranceReduceRate,
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