using System.Collections.Generic;
using BaseTools;
using Inventory;
using UnityEngine;

namespace CoreListener
{
    public class InventoryInfo : SingletonMono<InventoryInfo>
    {
        private readonly Dictionary<int, ItemData> _inventory = new();
        public ItemDataBase itemDataBase;

        protected override void Awake()
        {
            // 初始化物品数据字典 并读取所有物品数据

            #region 临时措施

            foreach (var item in itemDataBase.items)
            {
                if (_inventory.ContainsKey(item.itemID))
                    Debug.LogError($"存在相同的物品ID: {item.itemID}，请检查物品数据表！");

                _inventory.TryAdd(item.itemID, item);
            }

            Debug.Log("ok");

            #endregion
        }

        /// <summary>
        /// 创建一个物品实例
        /// </summary>
        /// <param name="itemID">物品ID</param>
        /// <param name="stackCount">创建的数量</param>
        /// <returns></returns>
        public Item GetItem(int itemID, int stackCount)
        {
            if (_inventory.TryGetValue(itemID, out var itemData))
            {
                if (!itemData.canStack)
                    stackCount = 1;

                return new Item(itemData)
                {
                    ItemID = itemID,
                    ItemStackCount = stackCount
                };
            }

            Debug.LogWarning($"Item ID {itemID} not found in inventory.");
            return null;
        }
    }
}