using System;
using System.Collections.Generic;
using Inventory.Save;
using UnityEngine;

namespace Inventory.Property
{
    public class DurableProperty : ItemProperty, ISerializableProperty
    {
        // Data数据
        public float MaxDurability { get; private set; } // 最大耐久度 

        // 运行数据
        public float CurrentDurability { get; private set; } // 当前耐久度 
        public bool IsBroken => CurrentDurability <= 0;

        public DurableProperty(float maxDurability)
        {
            MaxDurability = maxDurability;
            CurrentDurability = maxDurability;
        }

        #region 操作逻辑

        //todo: 使用 / 修复  对物品本身的操作  对于合成这类创造性的操作应由外界实现

        #endregion


        #region 序列化逻辑

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">值</param>
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

        /// <summary>
        /// 序列化动态属性
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> SerializeState()
        {
            var state = new Dictionary<string, object>()
            {
                [nameof(CurrentDurability)] = CurrentDurability,
            };
            return state;
        }

        /// <summary>
        /// 内容反序列化
        /// </summary>
        /// <param name="serializedData"></param>
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