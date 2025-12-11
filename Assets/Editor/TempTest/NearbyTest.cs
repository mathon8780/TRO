using System.Collections.Generic;
using CoreListener;
using Inventory;
using Inventory.Property;
using Inventory.WorldInteract;
using UnityEditor;
using UnityEngine;

namespace Editor.TempTest
{
    public class NearbyTest
    {
        [MenuItem("Test/NearbyTest")] // 添加选项 为当前的内容创建窗口
        public static void TestFunc()
        {
            Item boxItem = InventoryInfo.Instance.GetItem(100010, 1); // 获取一个容器物品
            Item breadItem = InventoryInfo.Instance.GetItem(100003, 5); // 获取一个面包物品
            Item appleItem = InventoryInfo.Instance.GetItem(100002, 3); // 获取一个苹果物品
            Item axeItem = InventoryInfo.Instance.GetItem(100011, 1); // 获取一个斧头物品
            Item axe2Item = InventoryInfo.Instance.GetItem(100013, 1); // 获取另一个斧头物品
            List<Item> itemsToStore = new List<Item> { breadItem, appleItem, axeItem, axe2Item };
            boxItem.GetProperty<ItemContainerProperty>().StoreItem(itemsToStore);

            GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.name = "Box";
            box.AddComponent<WorldItem>().item = boxItem;
            box.gameObject.transform.position = new Vector3(2, 1, 2);
            box.tag = "WorldItem";
            box.AddComponent<Rigidbody>().useGravity = false;
        }
    }
}