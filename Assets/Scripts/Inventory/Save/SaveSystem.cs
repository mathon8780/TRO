using System.Collections.Generic;
using System.IO;
using System.Linq;
using BaseTools;
using UnityEditor;
using UnityEngine;

namespace Inventory.Save
{
    public class SaveSystem : SingletonMono<SaveSystem>
    {
        private static string dbPath = "Assets/Configs/Inventory/Items/ItemPropertyDataBase.asset";
        public ItemDataBase propertyDataBase;

        /// <summary>
        /// 序列化物品信息为存储格式
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public List<SavedItemData> SerializeItemsInfo(List<Item> items)
        {
            List<SavedItemData> savedItems = new List<SavedItemData>(items.Count);
            foreach (var item in items)
            {
                var savedItem = new SavedItemData()
                {
                    ItemID = item.ItemID,
                    StackCount = item.ItemStackCount,
                };
                //检查所有属性内容

                // ——通用序列化逻辑——
                // 序列化保存所有物品的动态属性
                foreach (var property in item.GetAllProperties())
                {
                    if (property is ISerializableProperty serializableProperty)
                    {
                        // 得到物品的所有属性
                        Dictionary<string, object> propertyState = serializableProperty.SerializeState();

                        // 遍历所有属性并添加到属性存储字典中
                        foreach (var pair in propertyState)
                        {
                            // 如果属性字典中不存在该属性，则添加
                            if (!savedItem.DynamicProperties.ContainsKey(pair.Key))
                            {
                                savedItem.DynamicProperties.Add(pair.Key, pair.Value);
                            }
                            // 如果属性字典中已经存在该属性，则打印警告
                            else
                            {
                                Debug.LogWarning("Occur repeat key: " + pair.Key + "-" + pair.Value);
                            }
                        }
                    }
                }

                // 将一个物品的存档信息添加到序列化savedItems列表中
                savedItems.Add(savedItem);
            }

            return savedItems;
        }

        /// <summary>
        /// 反序列化物品信息为实例格式
        /// </summary>
        /// <param name="savedItems"></param>
        public List<Item> DeserializeItemsInfo(List<SavedItemData> savedItems)
        {
            List<Item> items = new List<Item>(savedItems.Count);
            foreach (var savedItem in savedItems)
            {
                ItemData itemData = GetItemData(savedItem.ItemID); //todo:在物品中心获取对应ID的物品数据
                if (itemData == null)
                {
                    Debug.LogWarning($"Exist unknow id {savedItem.ItemID}");
                }

                Item item = new Item(itemData);

                item.ItemID = savedItem.ItemID;
                item.ItemStackCount = savedItem.StackCount;
                foreach (var property in item.GetAllProperties())
                {
                    if (property is ISerializableProperty serializableProperty)
                    {
                        serializableProperty.DeserializeState(savedItem.DynamicProperties);
                    }
                }

                items.Add(item);
            }

            return items;
        }


        public ItemData GetItemData(int itemID)
        {
            if (propertyDataBase == null)
            {
                CheckItemData();
            }

            return propertyDataBase.items.FirstOrDefault(p => p.itemID == itemID);
        }

        private void CheckItemData()
        {
            if (!File.Exists(dbPath))
            {
                Debug.LogError($"{dbPath} 不存在");
                return;
            }

            propertyDataBase = AssetDatabase.LoadAssetAtPath<ItemDataBase>(dbPath);
        }
    }
}