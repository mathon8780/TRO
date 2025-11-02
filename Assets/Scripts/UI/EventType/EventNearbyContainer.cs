using Inventory;

namespace UI.EventType
{
    /// <summary>
    /// Nearby Event
    /// 容器交互事件
    /// </summary>
    public class EventNearbyContainer
    {
        public bool IsClose;
        public Item ContainerItem;
    }

    /// <summary>
    /// Nearby Event
    /// 物品交互事件
    /// </summary>
    public class EventNearbyItem
    {
        public bool IsClose;
        public Item WorldItem;
    }
}