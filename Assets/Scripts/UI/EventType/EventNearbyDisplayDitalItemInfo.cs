using Inventory;
using UnityEditor;

namespace UI.EventType
{
    /// <summary>
    /// 显示物品详细信息事件
    /// </summary>
    public struct EventNearbyDisplayDitalItemInfo
    {
        public bool Display; // 是否显示详细信息
        public Item Item; // 物品
    }
}