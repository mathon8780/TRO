using System;
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
        public int ItemID = -1; // 物品ID
        public int ItemStackCount = -1; //物品数量
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

        /// <summary>
        /// 分离出指定数量的该物品，返回一个新的物品实例
        /// </summary>
        /// <param name="separateNum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Item DeepCopy(int separateNum)
        {
            if (separateNum <= 0)
                throw new System.ArgumentOutOfRangeException(nameof(separateNum), "separateNum must be greater than 0.");
            if (separateNum > ItemStackCount)
                throw new System.ArgumentOutOfRangeException(nameof(separateNum), "separateNum cannot exceed current ItemStackCount.");

            // 用 ItemData 构造新物品（会创建新的属性实例并 Initialize）
            Item newItem = new Item(ItemData)
            {
                ItemID = this.ItemID,
                ItemStackCount = separateNum
            };

            // 复制可序列化属性的运行时状态（若实现了 ISerializableProperty）
            foreach (var prop in ItemProperties)
            {
                if (prop is Save.ISerializableProperty serializable)
                {
                    var state = serializable.SerializeState();
                    var newProp = newItem.ItemProperties.FirstOrDefault(p => p.GetType() == prop.GetType());
                    if (newProp is Save.ISerializableProperty serializableNew && state != null)
                    {
                        serializableNew.DeserializeState(state);
                    }
                }
            }

            // 从当前物品扣除数量
            ItemStackCount -= separateNum;

            return newItem;
        }
    }
}