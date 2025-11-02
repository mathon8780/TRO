using System.Collections.Generic;
using System.Linq;
using Inventory;
using Inventory.Property;
using Inventory.Save;
using UnityEditor;

namespace Editor
{
    public class NewInventoryTestFunc
    {
        /// <summary>
        /// 测试物品的序列化与反序列化
        /// </summary>
        [MenuItem("Test/New Inventory Test serializable")] // 添加选项 为当前的内容创建窗口
        public static void TestFunc()
        {
            string dbPath = "Assets/Configs/NewInventory/Items/ItemPropertyDataBase.asset";
            ItemDataBase propertyDataBase = AssetDatabase.LoadAssetAtPath<ItemDataBase>(dbPath);

            ItemData itemData11 = propertyDataBase.items.FirstOrDefault(p => p.itemID == 100011);
            Item item11 = new Item(itemData11);
            item11.ItemID = itemData11.itemID;
            item11.ItemStackCount = 1;
            item11.GetProperty<DurableProperty>().SetPropertyData("DurableProperty", 1);
            ItemData itemData12 = propertyDataBase.items.FirstOrDefault(p => p.itemID == 100012);
            Item item12 = new Item(itemData12);
            item12.ItemID = itemData12.itemID;
            item12.ItemStackCount = 1;
            item12.GetProperty<DurableProperty>().SetPropertyData("DurableProperty", 2);
            ItemData itemData13 = propertyDataBase.items.FirstOrDefault(p => p.itemID == 100013);
            Item item13 = new Item(itemData13);
            item13.ItemID = itemData13.itemID;
            item13.ItemStackCount = 1;
            item13.GetProperty<DurableProperty>().SetPropertyData("DurableProperty", 3);
            ItemData itemData14 = propertyDataBase.items.FirstOrDefault(p => p.itemID == 100014);
            Item item14 = new Item(itemData14);
            item14.ItemID = itemData14.itemID;
            item14.ItemStackCount = 1;
            item14.GetProperty<DurableProperty>().SetPropertyData("DurableProperty", 4);
            List<Item> items = new List<Item>() { item11, item12, item13, item14 };
            List<SavedItemData> savedItems = SaveSystem.Instance.SerializeItemsInfo(items);
            List<Item> deserializeItems = SaveSystem.Instance.DeserializeItemsInfo(savedItems);
        }
    }
}