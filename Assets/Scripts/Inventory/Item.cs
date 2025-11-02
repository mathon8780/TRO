using System.Collections.Generic;
using System.Linq;
using Inventory.Property;
using Inventory.PropertyData;
using Newtonsoft.Json;

namespace Inventory
{
    /// <summary>
    /// 一个存储运行时物品所有信息的容器
    /// </summary>
    public class Item
    {
        public int ItemID; // 物品ID
        public int ItemStackCount; //物品数量
        [JsonIgnore] public ItemData ItemData { get; private set; } //物品的基础数据
        [JsonIgnore] public List<ItemProperty> ItemProperties = new(); //物品属性存储

        public Item(ItemData itemData)
        {
            ItemData = itemData;
            foreach (ItemPropertyData itemDataProperty in ItemData.properties)
            {
                var property = itemDataProperty.CreateProperty();
                property.Initialize(this);
                ItemProperties.Add(property);
            }
        }

        public T GetProperty<T>() where T : ItemProperty
        {
            return ItemProperties.FirstOrDefault(p => p is T) as T;
        }

        public IEnumerable<ItemProperty> GetAllProperties()
        {
            return ItemProperties;
        }
    }
}