using System.Collections.Generic;

namespace Inventory.Save
{
    public class SavedItemData
    {
        public int ItemID;
        public int StackCount; // 如果物品可堆叠

        // 用一个字典来存储所有动态属性，非常灵活
        public Dictionary<string, object> DynamicProperties;

        public SavedItemData()
        {
            DynamicProperties = new Dictionary<string, object>();
        }
    }
}