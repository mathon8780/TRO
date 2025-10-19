using System;
using System.Collections.Generic;

namespace ItemSys
{
    [Serializable]
    public class ItemInstance
    {
        public string itemId; // 指向 ItemData 的 ID
        public int stackCount = 1;

        public float currentDurability = 100f;

        // 运行时缓存：避免频繁查找
        [NonSerialized] public Item data;

        [NonSerialized] public List<ItemBehavior> behaviors;

        // 构造函数：从模板创建实例
        public static ItemInstance CreateFrom(Item item, int count = 1)
        {
            return new ItemInstance
            {
                itemId = item.itemId,
                stackCount = count,
                currentDurability = item.maxDurability,
                data = item,
                behaviors = new List<ItemBehavior>(item.behaviors)
            };
        }

        // 使用行为
        public void Use(Player player)
        {
            foreach (var behavior in behaviors)
            {
                if (behavior.CanUse(player))
                {
                    behavior.Use(player, data);
                    break; // 或允许多行为同时触发
                }
            }
        }
    }
}