using System.Collections.Generic;
using Inventory.PropertyData;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// 物品的基础数据内容
    /// </summary>
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Inventory/Item Data", order = 0)]
    public class ItemData : ScriptableObject
    {
        public int itemID; //物品ID
        public string itemName; //物品名称
        [TextArea] public string itemDescription; //物品描述
        public float itemWeight; //物品重量
        public bool canStack; //是否可堆叠

        public Sprite icon;

        //允许多态实例化
        [SerializeReference] public List<ItemPropertyData> properties;
    }
}