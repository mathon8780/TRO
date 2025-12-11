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
        public float MaxCapacity; // 容量
        public float CurrentCapacity => GetCurrentCapacity();

        // 动态数据
        public List<Item> Content;

        public ItemContainerProperty(float maxCapacity)
        {
            MaxCapacity = maxCapacity;
            Content = new List<Item>();
        }

        #region 物品容器操作逻辑

        /// <summary>
        /// 获取当前背包的占用容量
        /// </summary>
        /// <returns></returns>
        private float GetCurrentCapacity()
        {
            float sum = 0;
            foreach (var item in Content)
            {
                sum += item.ItemStackCount * item.ItemData.itemWeight;
            }

            return sum;
        }

        /// <summary>
        /// 存放物品
        /// </summary>
        /// <param name="item"></param>
        /// <returns>存入状态</returns>
        public bool StoreItem(Item item)
        {
            //物品种类筛选 (可扩展为物品过滤器属性) 如果不符合则直接返回false

            //容量判定
            int maxStoreNum = (int)((MaxCapacity - CurrentCapacity) / item.ItemData.itemWeight);
            // 可以存放下全部物品
            if (maxStoreNum >= item.ItemStackCount)
            {
                TryStoreAndMergeItem(item);
                return true;
            }
            // 存可以存下的部分
            else
            {
                // 存下
                TryStoreAndMergeItem(item.DeepCopy(maxStoreNum));
                return false;
            }
        }

        public bool StoreItem(List<Item> items)
        {
            foreach (var item in items)
            {
                // 物品种类筛选 (可扩展为物品过滤器属性) 如果不符合则直接跳过
                bool isStored = StoreItem(item);
                if (!isStored)
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 尝试存储并合并同类物品
        /// </summary>
        /// <param name="storeItem"></param>
        /// <returns></returns>
        private void TryStoreAndMergeItem(Item storeItem)
        {
            if (!storeItem.ItemData.canStack)
            {
                Content.Add(storeItem);
                return;
            }

            foreach (Item content in Content)
            {
                //如果存在同类物品则合并
                if (content.ItemID != storeItem.ItemID) continue;
                content.ItemStackCount += storeItem.ItemStackCount;
                return;
            }

            //不存在同类物品则直接添加
            Content.Add(storeItem);
        }

        #endregion

        #region ISerializableProperty 序列化与反序列化

        /// <summary>
        /// 设置属性内容
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
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
        /// 获取属性内容
        /// </summary>
        /// <returns>属性名-内容</returns>
        public Dictionary<string, object> SerializeState()
        {
            var state = new Dictionary<string, object>()
            {
                [nameof(MaxCapacity)] = MaxCapacity,
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